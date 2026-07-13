using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories.WL.Master
{
    public class WLMasterBasicCompanyRepository : MasterRepositoryBase<WL_MasterCompanyBasicData>
    {
        public WLMasterBasicCompanyRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<WL_MasterCompanyBasicData>> GetPagedAsync(
            Expression<Func<WL_MasterCompanyBasicData, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<WL_MasterCompanyBasicData>, IOrderedQueryable<WL_MasterCompanyBasicData>>? orderBy = null,
            Func<IQueryable<WL_MasterCompanyBasicData>, IQueryable<WL_MasterCompanyBasicData>>? queryModifier = null)
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
                                 select new WL_MasterCompanyBasicData
                                 {
                                     Id = e.Id,
                                     UUID = e.UUID,
                                     IsActive = e.IsActive,
                                     TenantId = e.TenantId,
                                     TenantName = t != null ? t.TenantName : string.Empty,
                                     CompName = e.CompName,
                                     Phone = e.Phone,
                                     EmailId = e.EmailId,
                                     GoogleMapIframe = e.GoogleMapIframe,
                                     Address = e.Address,
                                     WebsiteLogo = e.WebsiteLogo,
                                     StickyLogo = e.StickyLogo,
                                     FooterLogo = e.FooterLogo,
                                 };

                    return joined;
                });
        }
    }
}
