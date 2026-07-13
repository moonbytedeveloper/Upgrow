using System.ComponentModel.DataAnnotations;

namespace UpgrowAdminPanel.Models.Support
{
    public class SupportTicketDetailsVM
    {
        public string? TicketUUID { get; set; }
        public string? TicketNumber { get; set; }
        public string? Subject { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string? Assignee { get; set; }
        public string? Status { get; set; }
        public List<TicketMessageVM>? Messages { get; set; } = new();
        [Required(ErrorMessage = "Required!")]
        public string? ReplyMessage { get; set; }
        public List<IFormFile>? Files { get; set; }
        public string? Otp { get; set; }

    }
    public class TicketMessageVM
    {
        public string? LineUUID { get; set; }
        public string? UserName { get; set; }
        public string? UserType { get; set; }
        public string? Message { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public List<string>? Url { get; set; } = new();
        public List<string>? UrlWithDomain { get; set; } = new();
    }
}
