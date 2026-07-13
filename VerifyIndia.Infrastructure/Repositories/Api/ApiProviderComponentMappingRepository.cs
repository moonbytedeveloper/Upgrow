using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Infrastructure.Repositories.Api
{
    public class ApiProviderComponentMappingRepository : MasterRepositoryBase<Api_ProviderComponentMapping>
    {
        public ApiProviderComponentMappingRepository(AppDbContext context) : base(context)
        {
        }
        public override async Task<PagedResult<Api_ProviderComponentMapping>> GetPagedAsync(
         Expression<Func<Api_ProviderComponentMapping, bool>>? filter,
        PaginationParams pagination,
         Func<IQueryable<Api_ProviderComponentMapping>, IOrderedQueryable<Api_ProviderComponentMapping>>? orderBy = null,
         Func<IQueryable<Api_ProviderComponentMapping>, IQueryable<Api_ProviderComponentMapping>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                  filter,
                pagination,
                orderBy, 
                query => from e in _context.Api_ProviderComponentMapping
                                  join a in _context.Master_Api
                                      on e.ApiUUID equals a.UUID into apiGroup
                                  from a in apiGroup.DefaultIfEmpty()
                                  join c in _context.Api_Components
                                     on e.ComponentUUID equals c.UUID into componentGroup
                                  from c in componentGroup.DefaultIfEmpty()
                                  select new Api_ProviderComponentMapping
                                  {
                                      UUID = e.UUID,
                                      IsActive = e.IsActive,
                                      ApiUUID = a != null ? a.ApiName : null,       // map title
                                      ComponentUUID = c != null ? c.Name : null, // map title
                                      Id = e.Id // for ordering
                                  });

        }

           }
 }
