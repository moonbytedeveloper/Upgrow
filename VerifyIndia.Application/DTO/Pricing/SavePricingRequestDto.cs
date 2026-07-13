using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Pricing
{
    public class SavePricingRequestDto
    {
        public string PriceType { get; set; }
        public string ProviderUUID { get; set; }

        public DateOnly EffectiveDate { get; set; }

        public List<SavePricingItemDto>
            PricingList
        { get; set; }
                = new();
    }
}
