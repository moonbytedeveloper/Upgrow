namespace Upgrow.Application.DTO.Pricing
{
    public class PricingRowDto
    {
        public string PricingUUID { get; set; }

        public string APIUUID { get; set; }

        public string APIName { get; set; }

        public string APICode { get; set; }

        public string CategoryName { get; set; }

        public string ProviderUUID { get; set; }

        public string ProviderName { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal? NewPrice { get; set; }
    }
}
