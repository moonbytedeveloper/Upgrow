using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Website;

namespace Upgrow.Infrastructure.Repositories.Website
{
    public class MasterWebsiteFAQRepository : MasterRepositoryBase<Website_MasterFAQ>
    {
        public MasterWebsiteFAQRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Website_MasterFAQ>> GetPagedAsync(
    Expression<Func<Website_MasterFAQ, bool>>? filter,
    PaginationParams pagination,
    Func<IQueryable<Website_MasterFAQ>, IOrderedQueryable<Website_MasterFAQ>>? orderBy = null,
    Func<IQueryable<Website_MasterFAQ>, IQueryable<Website_MasterFAQ>>? queryModifier = null)
        {
            // Join FAQ with its Category and SubCategory tables so the DTO fields contain names (not UUIDs)
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from f in _context.Website_MasterFAQ
                         join cat in _context.Website_FAQCategory
                             on f.FAQCategoryUUID equals cat.UUID into catGroup
                         from cat in catGroup.DefaultIfEmpty()
                             // <- FIX: join against Master_FAQSubCategory (was incorrectly joining Service_Category)
                         
                         select new Website_MasterFAQ
                         {
                             Id = f.Id,
                             UUID = f.UUID,
                             Title = f.Title,
                             Description = f.Description,
                             // Map category/title into the FAQCategoryUUID field so controller can display the name
                             FAQCategoryUUID = cat != null ? cat.Title : null,
                             // Map subcategory/title into the FAQSubCategoryUUID field so controller can display the name
                             
                             IsActive = f.IsActive
                         });
        }
    }
}

    