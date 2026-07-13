using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Pricing
{
    public class ApiPricingProjectionDto
    {
        public string APIUUID { get; set; }

        public string APIName { get; set; }

        public string APICode { get; set; }

        public string CategoryUUID { get; set; }
        public string CategoryName { get; set; }

        public string ProviderUUID { get; set; }

        public string ProviderName { get; set; }

        public decimal? CurrentPrice { get; set; }
        public decimal? CurrentPriceMB { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string PricingUUID { get; set; }

        public DateOnly EffectiveFrom { get; set; }

        public DateOnly? EffectiveTo { get; set; }
    }
}
