using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Pricing;

namespace Upgrow.Application.IServices.Pricing
{
    public interface IMasterPricingService
    {
        Task<(bool success,
              string message,
              List<PricingGroupDto> data)>
              GetPriceTblAsync(
                  string priceType,
                  string providerUUID,
                  DateOnly effectiveDate);

        Task<(bool success,
              string message,
              List<PricingGroupDto> data)>
              GetCustomerPriceTblAsync(
                  string priceType,
                  string platformOwnerUuid,
                  DateOnly effectiveDate);

        Task<(bool success,
              string message,
              List<PricingGroupDto> data)>
              GetWhiteLabelPriceTblAsync(
                  string priceType,
                  string platformOwnerUuid,
                  DateOnly effectiveDate);

        Task<(bool success,
              string message)>
              SavePricingAsync(
                  SavePricingRequestDto request);

        Task<(bool success,
              string message)>
              SaveCustomerPricingAsync(
                  SavePricingRequestDto request);

        Task<(bool success,
              string message)>
              SaveWhiteLabelPricingAsync(
                  SavePricingRequestDto request);
    }
}
