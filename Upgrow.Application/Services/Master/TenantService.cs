using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        /// <summary>
        /// Get tenant name by tenant ID
        /// </summary>
        public async Task<TenantDto> GetTenantNameByIdAsync(int tenantId)
        {
            var tenantName = await _tenantRepository.GetTenantNameByIdAsync(tenantId);
            return new TenantDto { TenantName = tenantName };
        }
    }
}
