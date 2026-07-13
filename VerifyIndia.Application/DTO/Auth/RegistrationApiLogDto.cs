namespace VerifyIndia.Application.DTO.Auth
{
    public class RegistrationApiLogDto
    {
        public string? UUID { get; set; }
        public string? CustomerUUID { get; set; }
        public string? MobileNo { get; set; }        
        public int? TenantId { get; set; }
        public string? ApiPath { get; set; }
        public string? HttpMethod { get; set; }
        public string? ApiRequest { get; set; }
        public string? ApiResponse { get; set; }
        public int? StatusCode { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? IpAddress { get; set; }
    }
}
