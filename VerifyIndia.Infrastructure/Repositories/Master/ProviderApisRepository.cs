using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories.Master
{
    public class ProviderApisRepository : MasterRepositoryBase<Provider_Apis>
    {
        public ProviderApisRepository(AppDbContext context) : base(context)
        {
        }
        
        /// <summary>
        /// Override GetPagedAsync to include LEFT JOIN with Master_Api and Api_Provider
        /// Maps API name and Provider name for display instead of UUIDs
        /// </summary>
        public override async Task<PagedResult<Provider_Apis>> GetPagedAsync(
            Expression<Func<Provider_Apis, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Provider_Apis>, IOrderedQueryable<Provider_Apis>>? orderBy = null,
            Func<IQueryable<Provider_Apis>, IQueryable<Provider_Apis>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from providerApi in _context.ProviderApis
                         join api in _context.Master_Api
                             on providerApi.ApiUUID equals api.UUID into apiGroup
                         from api in apiGroup.DefaultIfEmpty()
                         join provider in _context.Api_Provider
                             on providerApi.ProviderUUID equals provider.UUID into providerGroup
                         from provider in providerGroup.DefaultIfEmpty()
                         select new Provider_Apis
                         {
                             Id = providerApi.Id,
                             UUID = providerApi.UUID,
                             ApiUUID = api != null ? api.ApiName : providerApi.ApiUUID, // Display API name
                             ProviderUUID = provider != null ? provider.ProviderName : providerApi.ProviderUUID, // Display Provider name
                             IsActive = providerApi.IsActive
                         });
        }
    }
}
