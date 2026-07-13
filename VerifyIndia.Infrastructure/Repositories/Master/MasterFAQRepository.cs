using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories.Master
{
    public class MasterFAQRepository : MasterRepositoryBase<Master_FAQ>
    {
        public MasterFAQRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Master_FAQ>> GetPagedAsync(
    Expression<Func<Master_FAQ, bool>>? filter,
    PaginationParams pagination,
    Func<IQueryable<Master_FAQ>, IOrderedQueryable<Master_FAQ>>? orderBy = null,
    Func<IQueryable<Master_FAQ>, IQueryable<Master_FAQ>>? queryModifier = null)
        {
            // Join FAQ with its Category and SubCategory tables so the DTO fields contain names (not UUIDs)
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from f in _context.Master_FAQ
                         join cat in _context.Master_FAQCategory
                             on f.FAQCategoryUUID equals cat.UUID into catGroup
                         from cat in catGroup.DefaultIfEmpty()
                             // <- FIX: join against Master_FAQSubCategory (was incorrectly joining Service_Category)
                         join sub in _context.Master_FAQSubCategory
                             on f.FAQSubCategoryUUID equals sub.UUID into subGroup
                         from sub in subGroup.DefaultIfEmpty()
                         select new Master_FAQ
                         {
                             Id = f.Id,
                             UUID = f.UUID,
                             Title = f.Title,
                             Description = f.Description,
                             // Map category/title into the FAQCategoryUUID field so controller can display the name
                             FAQCategoryUUID = cat != null ? cat.Title : null,
                             // Map subcategory/title into the FAQSubCategoryUUID field so controller can display the name
                             FAQSubCategoryUUID = sub != null ? sub.Title : null,
                             IsActive = f.IsActive
                         });
        }
    }
}

    