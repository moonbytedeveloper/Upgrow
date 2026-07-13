using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace VerifyIndia.Infrastructure.Repositories.WL.Master
{
    public class WLTestimonialRepository : MasterRepositoryBase<WL_MasterTestimonial>
    {
        public WLTestimonialRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<WL_MasterTestimonial>> GetPagedAsync(
            Expression<Func<WL_MasterTestimonial, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<WL_MasterTestimonial>, IOrderedQueryable<WL_MasterTestimonial>>? orderBy = null,
            Func<IQueryable<WL_MasterTestimonial>, IQueryable<WL_MasterTestimonial>>? queryModifier = null)
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

                    // left join with Tenant to pull TenantName for display
                    var joined = from e in query
                                 join t in _context.Tenant.AsNoTracking()
                                     on (decimal?)e.TenantId equals t.Id into tenantGroup
                                 from t in tenantGroup.DefaultIfEmpty()
                                 select new WL_MasterTestimonial
                                 {
                                     Id = e.Id,
                                     UUID = e.UUID,
                                     IsActive = e.IsActive,
                                     // keep TenantId from entity; TenantName set from joined tenant if present
                                     TenantId = e.TenantId,
                                     CustomerName = e.CustomerName,
                                     CompanyName = e.CompanyName,
                                     Comment = e.Comment,
                                     FilePath = e.FilePath,
                                     Star = e.Star,
                                     SequenceNo = e.SequenceNo,
                                     TenantName = t != null ? t.TenantName : string.Empty
                                 };

                    return joined;
                });
        }
    }
}