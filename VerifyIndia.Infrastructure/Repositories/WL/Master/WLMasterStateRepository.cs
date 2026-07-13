using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Infrastructure.Repositories.WL.Master
{
    public class WLMasterStateRepository : MasterRepositoryBase<WL_MasterState>
    {
        public WLMasterStateRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<WL_MasterState>> GetPagedAsync(
            Expression<Func<WL_MasterState, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<WL_MasterState>, IOrderedQueryable<WL_MasterState>>? orderBy = null,
            Func<IQueryable<WL_MasterState>, IQueryable<WL_MasterState>>? queryModifier = null)
        {
            // Join states with countries and map country title into CountryUUID field
            // This approach is identical to MasterFAQRepository
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from s in _context.WL_MasterState
                         join c in _context.WL_MasterCountry
                             on s.CountryUUID equals c.UUID into countryGroup
                         from c in countryGroup.DefaultIfEmpty()
                         select new WL_MasterState
                         {
                             Id = s.Id,
                             UUID = s.UUID,
                             CountryUUID = c != null ? c.Title : s.CountryUUID,  // mapped country name
                             Title = s.Title,
                             ShortTitle = s.ShortTitle,
                             IsActive = s.IsActive
                         });
        }
    }
}