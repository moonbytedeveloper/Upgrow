using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.MultiTenancy
{
    public class TenantStore : IMultiTenantStore<AppTenantInfo>
    {
        private readonly AppDbContext _db;

        public TenantStore(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> TryAddAsync(AppTenantInfo tenantInfo)
        {
            var tenant = new Tenant
            {
                Identifier = tenantInfo.Identifier,
                TenantName = tenantInfo.Name
            };

            _db.Tenant.Add(tenant);
            var result = await _db.SaveChangesAsync();
            
            tenantInfo.Id = tenant.Id.ToString();
            
            return result > 0;
        }

        public async Task<bool> TryUpdateAsync(AppTenantInfo tenantInfo)
        {
            var tenant = await _db.Tenant
                .FirstOrDefaultAsync(x => x.Id.ToString() == tenantInfo.Id);

            if (tenant == null) return false;

            tenant.Identifier = tenantInfo.Identifier;
            tenant.TenantName = tenantInfo.Name;

            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> TryRemoveAsync(string identifier)
        {
            var normalizedIdentifier = NormalizeIdentifier(identifier);

            var tenant = await _db.Tenant
                .FirstOrDefaultAsync(x => x.Identifier.ToLower() == normalizedIdentifier);

            if (tenant == null) return false;

            _db.Tenant.Remove(tenant);
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        //public async Task<AppTenantInfo> TryGetByIdentifierAsync(string identifier)
        //{
        //    var normalizedIdentifier = NormalizeIdentifier(identifier);

        //    var tenant = await _db.Tenant
        //        .FirstOrDefaultAsync(x => x.Identifier.ToLower() == normalizedIdentifier);

        //    if (tenant == null)
        //    {
        //        tenant = await _db.TenantDomain
        //            .Where(x => x.IsActive && x.Domain.ToLower() == normalizedIdentifier)
        //            .Select(x => x.Tenant)
        //            .FirstOrDefaultAsync();
        //    }

        //    if (tenant == null) return null;

        //    return new AppTenantInfo
        //    {
        //        Id = tenant.Id.ToString(),
        //        Identifier = tenant.Identifier,
        //        Name = tenant.TenantName
        //    };
        //}

        public async Task<AppTenantInfo> TryGetByIdentifierAsync(string identifier)
        {
            var normalizedIdentifier = NormalizeIdentifier(identifier);

            var tenant = await _db.Tenant
                .FirstOrDefaultAsync(x => x.Identifier.ToLower() == normalizedIdentifier);

            if (tenant == null)
            {
                // Previously used navigation: .Select(x => x.Tenant)
                // Now do a two-step lookup: get TenantId from TenantDomain, then fetch Tenant
                var tenantIdFromDomain = await _db.TenantDomain
                    .Where(x => x.IsActive && x.Domain.ToLower() == normalizedIdentifier)
                    .Select(x => x.TenantId)
                    .FirstOrDefaultAsync();

                if (tenantIdFromDomain != 0)
                {
                    // Convert int to decimal to match Tenant.Id CLR type
                    var tenantIdDecimal = Convert.ToDecimal(tenantIdFromDomain);

                    tenant = await _db.Tenant
                        .FirstOrDefaultAsync(t => t.Id == tenantIdDecimal);
                }
            }

            if (tenant == null) return null;

            return new AppTenantInfo
            {
                Id = tenant.Id.ToString(),
                Identifier = tenant.Identifier,
                Name = tenant.TenantName
            };
        }

        public async Task<AppTenantInfo> TryGetAsync(string id)
        {
            var tenant = await _db.Tenant
                .FirstOrDefaultAsync(x => x.Id.ToString() == id);

            if (tenant == null) return null;

            return new AppTenantInfo
            {
                Id = tenant.Id.ToString(),
                Identifier = tenant.Identifier,
                Name = tenant.TenantName
            };
        }

        public async Task<IEnumerable<AppTenantInfo>> GetAllAsync()
        {
            var tenants = await _db.Tenant.ToListAsync();

            return tenants.Select(tenant => new AppTenantInfo
            {
                Id = tenant.Id.ToString(),
                Identifier = tenant.Identifier,
                Name = tenant.TenantName
            });
        }

        public async Task<IEnumerable<AppTenantInfo>> GetAllAsync(int take, int skip)
        {
            var tenants = await _db.Tenant
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return tenants.Select(tenant => new AppTenantInfo
            {
                Id = tenant.Id.ToString(),
                Identifier = tenant.Identifier,
                Name = tenant.TenantName
            });
        }

        private static string NormalizeIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return string.Empty;

            var value = identifier.Trim().ToLowerInvariant();

            var schemeIndex = value.IndexOf("://", StringComparison.Ordinal);
            if (schemeIndex >= 0)
                value = value[(schemeIndex + 3)..];

            var slashIndex = value.IndexOf('/');
            if (slashIndex >= 0)
                value = value[..slashIndex];

            var portIndex = value.IndexOf(':');
            if (portIndex >= 0)
                value = value[..portIndex];

            return value;
        }
    }
}
