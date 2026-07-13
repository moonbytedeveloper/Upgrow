using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Verification;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Domain.IRepositories.Master;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VerifyIndia.Infrastructure.Repositories.WL.Master
{
    public class WLMasterCmsRepository : MasterRepositoryBase<WL_MasterCMS>
    {
        public WLMasterCmsRepository(AppDbContext context) : base(context)
        {
        }
        public override async Task<PagedResult<WL_MasterCMS>> GetPagedAsync(
        Expression<Func<WL_MasterCMS, bool>>? filter,
       PaginationParams pagination,
        Func<IQueryable<WL_MasterCMS>, IOrderedQueryable<WL_MasterCMS>>? orderBy = null,
        Func<IQueryable<WL_MasterCMS>, IQueryable<WL_MasterCMS>>? queryModifier = null)
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
                                     on (decimal)e.TenantId equals t.Id into tenantGroup
                                 from t in tenantGroup.DefaultIfEmpty()
                                 select new WL_MasterCMS
                                 {
                                     Id = e.Id,
                                     UUID = e.UUID,
                                     IsActive = e.IsActive,
                                     PageTitle = e.PageTitle,
                                     Description = e.Description,
                                     UploadImage = e.UploadImage,
                                     TenantId = e.TenantId,
                                     TenantName = t != null ? t.TenantName : string.Empty
                                 };
                    return joined;
                });
        }
    }
}
