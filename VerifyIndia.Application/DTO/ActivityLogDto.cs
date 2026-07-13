namespace VerifyIndia.Application.DTO
{
    /// <summary>
    /// DTO for activity logs with employee information
    /// </summary>
    public class ActivityLogDto
    {
        
        public string UserUUID { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MenuName { get; set; } = string.Empty;
        public string PageUrl { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
     

    }
}
