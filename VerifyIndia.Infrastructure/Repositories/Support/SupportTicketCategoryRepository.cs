using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Repositories.Support
{
    public class SupportTicketCategoryRepository : MasterRepositoryBase<Support_TicketCategory>
    {
        public SupportTicketCategoryRepository(AppDbContext context) : base(context)
        {
        }
        public override async Task<PagedResult<Support_TicketCategory>> GetPagedAsync(
        Expression<Func<Support_TicketCategory, bool>>? filter,
       PaginationParams pagination,
        Func<IQueryable<Support_TicketCategory>, IOrderedQueryable<Support_TicketCategory>>? orderBy = null,
        Func<IQueryable<Support_TicketCategory>, IQueryable<Support_TicketCategory>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                  filter,
                pagination,
                orderBy, query => from s in _context.Support_TicketCategory
                                  join d in _context.Master_Designation
                                      on s.DesignationUUID equals d.UUID into designationGroup
                                  from r in designationGroup.DefaultIfEmpty()
                                  select new Support_TicketCategory
                                  {
                                      UUID = s.UUID,
                                      Title = s.Title,
                                      DesignationUUID = r.Title,
                                      UserType = s.UserType,
                                      IsActive = s.IsActive,
                                      Id = s.Id // for ordering
                                  });


        }
    }
}
