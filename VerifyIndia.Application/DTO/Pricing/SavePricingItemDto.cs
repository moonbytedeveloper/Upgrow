using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Pricing
{
    public class SavePricingItemDto
    {
        public string APIUUID { get; set; }

        public string? SellerTenantId { get; set; }

        public string ProviderUUID { get; set; }

        public string BuyerType { get; set; }

        public string BuyerUUID { get; set; }

        public decimal BaseAmount { get; set; }
        public decimal? BaseAmountMB { get; set; }
    }
}
