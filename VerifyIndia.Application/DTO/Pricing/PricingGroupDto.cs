namespace VerifyIndia.Application.DTO.Pricing
{
    public class PricingGroupDto
    {
        public string CategoryName { get; set; }

        public List<ApiPricingProjectionDto> APIs { get; set; }
    }
}
