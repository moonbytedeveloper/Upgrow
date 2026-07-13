using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;


namespace Upgrow.Infrastructure.Repositories.Master
{
    public class MasterBlogRepository : MasterRepositoryBase<Master_Blog>
    {
        public MasterBlogRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Master_Blog>> GetPagedAsync(
       Expression<Func<Master_Blog, bool>>? filter,
      PaginationParams pagination,
       Func<IQueryable<Master_Blog>, IOrderedQueryable<Master_Blog>>? orderBy = null,
       Func<IQueryable<Master_Blog>, IQueryable<Master_Blog>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy, query => from e in _context.Master_Blog
                                  join r in _context.Master_BlogCategory
                                      on e.BlogCategoryUUID equals r.UUID into bloggroup
                                  from r in bloggroup.DefaultIfEmpty()
                                  select new Master_Blog
                                  {
                                      UUID = e.UUID,
                                      BlogCategoryUUID = r != null ? r.Title : null, // mapped to title
                                      BlogImageUrl = e.BlogImageUrl,
                                      Title = e.Title,
                                      BlogDate = e.BlogDate,
                                      ShortDescription = e.ShortDescription,
                                      IsActive = e.IsActive,
                                      Id = e.Id // for ordering
                                  });
        }
    }
}
