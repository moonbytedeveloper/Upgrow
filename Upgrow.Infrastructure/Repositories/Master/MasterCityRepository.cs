using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Repositories.Master
{
    public class MasterCityRepository : MasterRepositoryBase<Master_City>
    {
        public MasterCityRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Master_City>> GetPagedAsync(
            Expression<Func<Master_City, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Master_City>, IOrderedQueryable<Master_City>>? orderBy = null,
            Func<IQueryable<Master_City>, IQueryable<Master_City>>? queryModifier = null)
        {
            // Join cities with states and countries and map state/country title into the UUID fields
            // so DTOs receive friendly names (same approach as MasterFAQRepository)
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from city in _context.Master_City
                         join st in _context.Master_State
                             on city.StateUUID equals st.UUID into stateGroup
                         from st in stateGroup.DefaultIfEmpty()
                         join c in _context.Master_Country
                             on city.CountryUUID equals c.UUID into countryGroup
                         from c in countryGroup.DefaultIfEmpty()
                         select new Master_City
                         {
                             Id = city.Id,
                             UUID = city.UUID,
                             CountryUUID = c != null ? c.Title : city.CountryUUID, // mapped country name
                             StateUUID = st != null ? st.Title : city.StateUUID,   // mapped state name
                             Title = city.Title,
                             ShortTitle = city.ShortTitle,
                             IsActive = city.IsActive
                         });
        }
    }
}