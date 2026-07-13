using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories
{
    public interface ITenantRepository
    {
            /// <summary>
            /// Get tenant name by tenant ID
            /// </summary>
            Task<string> GetTenantNameByIdAsync(int tenantId);
            Task<Tenant> GetTenantDetailsByIdentifierAsync(string identifier);
            Task<Tenant> GetTenantDetailsByIdAsync(decimal tenantId);
    }
}
