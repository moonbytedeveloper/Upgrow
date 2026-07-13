using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.WL;

namespace Upgrow.Application.IServices.Master
{
    public interface ITenantService
    {
        
        /// <summary>
        /// Get tenant name by tenant ID
        /// </summary>
        Task<TenantDto> GetTenantNameByIdAsync(int tenantId);
    }
}
