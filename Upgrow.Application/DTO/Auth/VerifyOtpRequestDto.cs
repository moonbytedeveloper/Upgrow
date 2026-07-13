namespace Upgrow.Application.DTO.Auth
{
    public class VerifyOtpRequestDto
    {
        public string MobileNumber { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
