using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Infrastructure.Repositories.WL.Master
{
    public class WLMasterBannerRepository : MasterRepositoryBase<WL_MasterBanner>
    {
        public WLMasterBannerRepository(AppDbContext context) : base(context)
        {
        }
        public override async Task<PagedResult<WL_MasterBanner>> GetPagedAsync(
        Expression<Func<WL_MasterBanner, bool>>? filter,
       PaginationParams pagination,
        Func<IQueryable<WL_MasterBanner>, IOrderedQueryable<WL_MasterBanner>>? orderBy = null,
        Func<IQueryable<WL_MasterBanner>, IQueryable<WL_MasterBanner>>? queryModifier = null)
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
                                 select new WL_MasterBanner
                                 {
                                     Id = e.Id,
                                     UUID = e.UUID,
                                     IsActive = e.IsActive,
                                     MainTitle = e.MainTitle,
                                     SubTitle = e.SubTitle,
                                     OptionalTitle = e.OptionalTitle,
                                     ButtonText = e.ButtonText,
                                     ButtonURL = e.ButtonURL,
                                     BannerImage = e.BannerImage,
                                     SequenceNo = e.SequenceNo,
                                     TenantId = e.TenantId,
                                     TenantName = t != null ? t.TenantName : string.Empty
                                 };
                    return joined;
                });
        }

    }
}
