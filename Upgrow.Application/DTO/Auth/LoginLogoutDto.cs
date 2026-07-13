namespace Upgrow.Application.DTO.Auth
{
    public class LoginLogoutDto
    {
        public string? UserUUID { get; set; }
        public string Activity { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public string IpAddress { get; set; } = null!;
    }
}