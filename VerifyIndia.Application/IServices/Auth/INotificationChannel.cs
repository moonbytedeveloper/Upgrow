using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.IServices.Auth
{
    public interface INotificationChannel
    {
        string ChannelName { get; }
        Task<NotificationChannelResult> SendAsync(NotificationContext context);
        bool CanHandle(NotificationContext context);
    }

    public class NotificationContext
    {
        public string RecipientId { get; set; } = "";
        public string RecipientEmail { get; set; } = "";
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public Dictionary<string, string> Placeholders { get; set; } = new();
        public Dictionary<string, string> CustomData { get; set; } = new();
        public decimal? EmailTemplateId { get; set; }
        public string? SenderName { get; set; }
        public List<string>? CcEmails { get; set; }
        public bool IsWhiteLabel { get; set; }
    }

    public class NotificationChannelResult
    {
        public string ChannelName { get; set; } = "";
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
        public DateTime SentAt { get; set; } = DateTime.Now;
        public Dictionary<string, object> Metadata { get; set; } = new();
        public Exception? Error { get; set; }
    }
}
