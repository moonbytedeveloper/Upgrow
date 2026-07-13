using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories.Api
{
    public class ApiEndpointRepository : MasterRepositoryBase<Api_Endpoint>
    {
        public ApiEndpointRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Api_Endpoint>> GetPagedAsync(
           Expression<Func<Api_Endpoint, bool>>? filter,
           PaginationParams pagination,
           Func<IQueryable<Api_Endpoint>, IOrderedQueryable<Api_Endpoint>>? orderBy = null,
           Func<IQueryable<Api_Endpoint>, IQueryable<Api_Endpoint>>? queryModifier = null)
        {
            // Project from Api_Endpoint and join provider & api to provide friendly display names.
            Func<IQueryable<Api_Endpoint>, IQueryable<Api_Endpoint>> joinProjection = _ =>
                from e in _context.Api_Endpoint
                join a in _context.Master_Api on e.ApiUUID equals a.UUID into apiGroup
                from a in apiGroup.DefaultIfEmpty()
                select new Api_Endpoint
                {
                    Id = e.Id,
                    UUID = e.UUID,
                    // Replace UUID fields with display names for UI
                    ApiUUID = a != null ? a.ApiName : e.ApiUUID,
                    EndpointUrl = e.EndpointUrl,
                    HttpMethod = e.HttpMethod,
                    IsActive = e.IsActive,
                };

            return await base.GetPagedAsync(filter, pagination, orderBy, joinProjection);
        }
    }

}
    
