using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.WL;

namespace VerifyIndia.Application.IServices.Master
{
    public interface ITenantService
    {
        
        /// <summary>
        /// Get tenant name by tenant ID
        /// </summary>
        Task<TenantDto> GetTenantNameByIdAsync(int tenantId);
    }
}
