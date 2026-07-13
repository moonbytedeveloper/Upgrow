using Finbuckle.MultiTenant.Abstractions;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Infrastructure.MultiTenancy
{
    public class TenantSetupService : ITenantSetupService
    {
        private readonly IMultiTenantContextAccessor<AppTenantInfo> _tenantAccessor;

        public TenantSetupService(IMultiTenantContextAccessor<AppTenantInfo> tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;
        }

        public int TenantId =>
            int.Parse(_tenantAccessor.MultiTenantContext?.TenantInfo?.Id ?? "0");

        public string Identifier =>
            _tenantAccessor.MultiTenantContext?.TenantInfo?.Identifier;
        
    }
}
