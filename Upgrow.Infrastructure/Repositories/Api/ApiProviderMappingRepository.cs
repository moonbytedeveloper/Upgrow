using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Infrastructure.Repositories.Api
{
    public class ApiProviderMappingRepository : MasterRepositoryBase<Api_ProviderMapping>
    {
        public ApiProviderMappingRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Api_ProviderMapping>> GetPagedAsync(
           Expression<Func<Api_ProviderMapping, bool>>? filter,
           PaginationParams pagination,
           Func<IQueryable<Api_ProviderMapping>, IOrderedQueryable<Api_ProviderMapping>>? orderBy = null,
           Func<IQueryable<Api_ProviderMapping>, IQueryable<Api_ProviderMapping>>? queryModifier = null)
        {
            // Project mapping rows so the UI receives friendly names instead of UUIDs.
            // ProviderUUID will carry ProviderName and ApiUUID will carry ApiName for display.
            Func<IQueryable<Api_ProviderMapping>, IQueryable<Api_ProviderMapping>> joinProjection = _ =>
                from m in _context.Api_ProviderMapping
                join p in _context.Api_Provider
                    on m.ProviderUUID equals p.UUID into prov
                from p in prov.DefaultIfEmpty()
                join a in _context.Master_Api
                    on m.ApiUUID equals a.UUID into apiGroup
                from a in apiGroup.DefaultIfEmpty()
                select new Api_ProviderMapping
                {
                    Id = m.Id,
                    UUID = m.UUID,
                    // Replace UUID fields with display names for UI
                    ProviderUUID = p != null ? p.ProviderName : m.ProviderUUID,
                    ApiUUID = a != null ? a.ApiName : m.ApiUUID,                
                    IsActive = m.IsActive,
                   
                };

            return await base.GetPagedAsync(filter, pagination, orderBy, joinProjection);
        }
    }

}