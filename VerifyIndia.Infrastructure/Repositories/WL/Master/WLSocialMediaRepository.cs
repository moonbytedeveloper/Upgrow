using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VerifyIndia.Infrastructure.Repositories.WL.Master
{
    public class WLSocialMediaRepository : MasterRepositoryBase<WL_MasterSocialMedia>
    {
        public WLSocialMediaRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<WL_MasterSocialMedia>> GetPagedAsync(
            Expression<Func<WL_MasterSocialMedia, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<WL_MasterSocialMedia>, IOrderedQueryable<WL_MasterSocialMedia>>? orderBy = null,
            Func<IQueryable<WL_MasterSocialMedia>, IQueryable<WL_MasterSocialMedia>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query =>
                {
                    if (queryModifier != null)
                    {
                        query = queryModifier(query);
                    }

                    var joined = from e in query
                                 join t in _context.Tenant.AsNoTracking()
                                     on (decimal?)e.TenantId equals t.Id into tenantGroup
                                 from t in tenantGroup.DefaultIfEmpty()
                                 select new WL_MasterSocialMedia
                                 {
                                     Id = e.Id,
                                     UUID = e.UUID,
                                     IsActive = e.IsActive,
                                     // if tenant is null (left join), keep original TenantId from e
                                     //TenantId = t != null ? (int)t.Id : e.TenantId,
                                     PlatformName = e.PlatformName,
                                     ProfileURL = e.ProfileURL,
                                     IconURL = e.IconURL,
                                     DisplayOrder = e.DisplayOrder,
                                     TenantId = e.TenantId,
                                      TenantName = t != null ? t.TenantName : string.Empty
                                 };
                    return joined;
                });
        }
    }
}