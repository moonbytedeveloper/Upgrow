using System.ComponentModel.DataAnnotations;

namespace Upgrow.Application.DTO.Pricing
{
    public class PricingFilterDto
    {
        [Required]
        public string PriceType { get; set; }
        [Required]
        public string ProviderUUID { get; set; }
        [Required]

        public string EffectiveDate { get; set; }
    }

    public class CWPricingFilterDto // Customer and Whitelabel pricing filter dto
    {
        [Required]
        public string PriceType { get; set; }

        [Required]

        public string EffectiveDate { get; set; }
    }
}
