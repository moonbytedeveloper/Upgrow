using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories
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
