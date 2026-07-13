namespace VerifyIndia.Application.DTO.Auth
{
    public class LoginAttemptDto
    {
        public string? UserUUID { get; set; }
        public string UserName { get; set; } = null!;
        public DateTimeOffset AttemptTime { get; set; }
        public bool IsSuccess { get; set; }
        public string FailureReason { get; set; } = null!;
        public string IpAddress { get; set; } = null!;

        public string? TenantId { get; set; }
    }
}