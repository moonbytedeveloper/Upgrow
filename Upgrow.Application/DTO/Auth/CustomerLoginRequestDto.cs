namespace Upgrow.Application.DTO.Auth
{
    public class CustomerLoginRequestDto
    {
        public string MobileNo { get; set; } = string.Empty;
        public int Otp { get; set; }
    }
}
