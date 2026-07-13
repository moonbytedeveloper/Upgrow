using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Enums;

namespace VerifyIndia.Application.DTO.Notification
{
    public sealed class NotificationMessageDto
    {
        public NotificationChannel Channel { get; set; }

        public string Recipient { get; set; }
            = string.Empty;

        public string EventCode { get; set; }
            = string.Empty;

        public string Subject { get; set; }
            = string.Empty;

        public string Body { get; set; }
            = string.Empty;

        public string? TemplateName { get; set; }

        public Dictionary<string, object> Variables
        {
            get;
            set;
        } = new();
    }
}
