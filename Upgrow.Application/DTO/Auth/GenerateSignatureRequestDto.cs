namespace Upgrow.Application.DTO.Auth
{
    public class GenerateSignatureRequestDto
    {
        public long Timestamp { get; set; }
        public object Payload { get; set; } = default!;
        public string ClientSecretHash { get; set; } = string.Empty;
    }
}
