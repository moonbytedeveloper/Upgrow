using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Pricing;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.Interfaces.Pricing
{
    public interface IMasterPricingRepository
    {
        IQueryable<ApiPricingProjectionDto> GetPricingQuery(DateOnly effectiveDate,
            string providerUUID);

        IQueryable<ApiPricingProjectionDto> GetCustomerPricingQuery(
            DateOnly effectiveDate,
            string platformOwnerUuid);

        IQueryable<ApiPricingProjectionDto> GetWhiteLabelPricingQuery(
            DateOnly effectiveDate,
            string platformOwnerUuid);

        Task<IQueryable<Master_Pricing>> GetProviderExistingPricing(string providerUUID);

        Task<IQueryable<Master_Pricing>> GetCustomerExistingPricing(
            string sellerTenantId);

        Task<IQueryable<Master_Pricing>> GetWhiteLabelExistingPricing(
            string sellerTenantId);

        Task AddAsync(Master_Pricing entity);

        void Update(Master_Pricing entity);

        Task SaveChangesAsync();
    }
}
