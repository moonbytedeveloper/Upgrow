using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Infrastructure.Repositories.Master
{
    public class MasterApiRepository : MasterRepositoryBase<Master_Api>, IMasterApiRepository
    {
        public MasterApiRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Master_Api>> GetByUuidsAsync(
            List<string> apiUuids)
        {
            return await _context.Master_Api
                .Where(x =>
                    apiUuids.Contains(x.UUID)
                    && x.IsActive)
                .ToListAsync();
        }

        public async Task<Master_Api?> GetByUuidAsync(
            string apiUuid)
        {
            return await _context.Master_Api
                .FirstOrDefaultAsync(x =>
                    x.UUID == apiUuid &&
                    x.IsActive);
        }

        /// <summary>
        /// Override GetPagedAsync to include LEFT JOIN with Api_Category
        /// Maps category name to ApiCategoryUUID for display
        /// </summary>
        public override async Task<PagedResult<Master_Api>> GetPagedAsync(
            Expression<Func<Master_Api, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Master_Api>, IOrderedQueryable<Master_Api>>? orderBy = null,
            Func<IQueryable<Master_Api>, IQueryable<Master_Api>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from api in _context.Master_Api
                         join category in _context.Api_Category
                             on api.ApiCategoryUUID equals category.UUID into categoryGroup
                         from category in categoryGroup.DefaultIfEmpty()
                         select new Master_Api
                         {
                             Id = api.Id,
                             UUID = api.UUID,
                             ApiName = api.ApiName,
                             Code = api.Code,
                             ApiCategoryUUID = category != null ? category.CategoryName : "N/A", // Map category name
                             ShortDescription = api.ShortDescription,
                             DisplayOrder = api.DisplayOrder,
                             IsConsentBased = api.IsConsentBased,
                             IsReminderRequired = api.IsReminderRequired,
                             IsProviderSwitchable =api.IsProviderSwitchable,
                             IsActive = api.IsActive
                         });
        }
    }
}
