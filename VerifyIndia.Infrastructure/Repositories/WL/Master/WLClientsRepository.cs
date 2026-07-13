using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VerifyIndia.Infrastructure.Repositories.WL.Master
{
    public class WLClientsRepository : MasterRepositoryBase<WL_Clients>
    {
        public WLClientsRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<WL_Clients>> GetPagedAsync(
            Expression<Func<WL_Clients, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<WL_Clients>, IOrderedQueryable<WL_Clients>>? orderBy = null,
            Func<IQueryable<WL_Clients>, IQueryable<WL_Clients>>? queryModifier = null)
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

                    // Left join with Tenant to populate TenantName for read/display
                    var joined = from e in query
                                 join t in _context.Tenant.AsNoTracking()
                                     on (decimal?)e.TenantId equals t.Id into tenantGroup
                                 from t in tenantGroup.DefaultIfEmpty()
                                 select new WL_Clients
                                 {
                                     Id = e.Id,
                                     UUID = e.UUID,
                                     IsActive = e.IsActive,
                                     TenantId = e.TenantId,
                                     Name = e.Name,
                                     IconImage = e.IconImage,
                                     TenantName = t != null ? t.TenantName : string.Empty
                                 };

                    return joined;
                });
        }
    }
}