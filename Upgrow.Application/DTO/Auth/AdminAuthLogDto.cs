namespace Upgrow.Application.DTO.Auth
{
    public class AdminAuthLogDto
    {
 
        public string? UserUUID { get; set; }
        public string? Activity { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}