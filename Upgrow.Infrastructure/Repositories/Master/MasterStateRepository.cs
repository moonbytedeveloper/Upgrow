using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Infrastructure.Repositories.Master
{
    public class WebsiteCatServiceRepository : MasterRepositoryBase<Master_State>
    {
        public WebsiteCatServiceRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Master_State>> GetPagedAsync(
            Expression<Func<Master_State, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Master_State>, IOrderedQueryable<Master_State>>? orderBy = null,
            Func<IQueryable<Master_State>, IQueryable<Master_State>>? queryModifier = null)
        {
            // Join states with countries and map country title into CountryUUID field
            // This approach is identical to MasterFAQRepository
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from s in _context.Master_State
                         join c in _context.Master_Country
                             on s.CountryUUID equals c.UUID into countryGroup
                         from c in countryGroup.DefaultIfEmpty()
                         select new Master_State
                         {
                             Id = s.Id,
                             UUID = s.UUID,
                             CountryUUID = c != null ? c.Title : s.CountryUUID, 
                             Title = s.Title,
                             ShortTitle = s.ShortTitle,
                             IsActive = s.IsActive
                         });
        }
    }
}