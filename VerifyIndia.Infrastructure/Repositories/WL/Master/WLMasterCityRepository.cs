using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories.WL.Master
{
    public class WLMasterCityRepository : MasterRepositoryBase<WL_MasterCity>
    {
        public WLMasterCityRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<WL_MasterCity>> GetPagedAsync(
            Expression<Func<WL_MasterCity, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<WL_MasterCity>, IOrderedQueryable<WL_MasterCity>>? orderBy = null,
            Func<IQueryable<WL_MasterCity>, IQueryable<WL_MasterCity>>? queryModifier = null)
        {
            // Join cities with states and countries and map state/country title into the UUID fields
            // so DTOs receive friendly names (same approach as MasterFAQRepository)
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from city in _context.WL_MasterCity
                         join st in _context.WL_MasterState 
                             on city.StateUUID equals st.UUID into stateGroup
                         from st in stateGroup.DefaultIfEmpty()
                         join c in _context.WL_MasterCountry
                             on city.CountryUUID equals c.UUID into countryGroup
                         from c in countryGroup.DefaultIfEmpty()
                         select new WL_MasterCity
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