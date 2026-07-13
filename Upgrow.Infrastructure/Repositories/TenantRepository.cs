using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Infrastructure.Repositories
{
    public class TenantRepository : MasterRepositoryBase<Tenant>, ITenantRepository
    {       

        public TenantRepository(AppDbContext context) : base(context)
        {
          
        }

        /// <summary>
        /// Get tenant name by tenant ID
        /// </summary>
        public async Task<string> GetTenantNameByIdAsync(int tenantId)
        {
            var tenantName = await _context.Tenant
                .Where(t => t.Id == tenantId)
                .Select(t => t.TenantName)
                .FirstOrDefaultAsync();

            return tenantName ?? tenantId.ToString();
        }
        public async Task<Tenant> GetTenantDetailsByIdentifierAsync(string identifier)
        {
            var tenantDetails = await _context.Tenant
                .Where(t => t.Identifier == identifier)
                .Select(t => new Tenant
                {
                    Id = t.Id,
                    UUID = t.UUID,
                    Identifier = t.Identifier,
                    TenantName = t.TenantName,
                    IsPlatformOwner = t.IsPlatformOwner,
                    IsActive = t.IsActive,
                    SecretHash = t.SecretHash
                })
                .FirstOrDefaultAsync();

            return tenantDetails;
        }
        public async Task<Tenant> GetTenantDetailsByIdAsync(decimal tenantId)
        {
            var tenantDetails = await _context.Tenant
                .Where(t => t.Id == tenantId)
                .Select(t => new Tenant
                {
                    Id = t.Id,
                    UUID = t.UUID,
                    Identifier = t.Identifier,
                    TenantName = t.TenantName,
                    IsPlatformOwner = t.IsPlatformOwner,
                    IsActive = t.IsActive,
                    SecretHash = t.SecretHash
                })
                .FirstOrDefaultAsync();

            return tenantDetails;
        }
    }
}