namespace Upgrow.Application.DTO.Auth
{
    public class HmacValidationResultDto
    {
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
        public string? TenantIdentifier { get; set; }
        public string? RawBody { get; set; }

    }
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
