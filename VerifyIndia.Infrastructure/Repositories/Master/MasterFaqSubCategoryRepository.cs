using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories.Master
{
    public class MasterFaqSubCategoryRepository : MasterRepositoryBase<Master_FAQSubCategory>
    {
        public MasterFaqSubCategoryRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Override GetPagedAsync to include LEFT JOIN with FAQ Category
        /// Maps category title to FAQCategoryUUID so the UI shows the category name
        /// </summary>
        public override async Task<PagedResult<Master_FAQSubCategory>> GetPagedAsync(
            Expression<Func<Master_FAQSubCategory, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Master_FAQSubCategory>, IOrderedQueryable<Master_FAQSubCategory>>? orderBy = null,
            Func<IQueryable<Master_FAQSubCategory>, IQueryable<Master_FAQSubCategory>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from sub in _context.Master_FAQSubCategory
                         join cat in _context.Master_FAQCategory
                             on sub.FAQCategoryUUID equals cat.UUID into catGroup
                         from cat in catGroup.DefaultIfEmpty()
                         select new Master_FAQSubCategory
                         {
                             Id = sub.Id,
                             UUID = sub.UUID,
                             // Put the category name into the FAQCategoryUUID property for display in admin UI
                             FAQCategoryUUID = cat != null ? cat.Title : "",
                             Title = sub.Title,
                             IsActive = sub.IsActive
                         });
        }
    }
}