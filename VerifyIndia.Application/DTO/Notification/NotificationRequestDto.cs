using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Enums;

namespace VerifyIndia.Application.DTO.Notification
{
    public sealed class NotificationRequestDto
    {
        public string EventCode { get; set; }
            = string.Empty;

        public string UserId { get; set; }
            = string.Empty;

        /// <summary>
        /// Optional channel override.
        /// Must be a subset of channels configured in NotificationRouting.
        /// When specified, user notification preferences are ignored.
        /// </summary>
        public IReadOnlyCollection<NotificationChannel>? Channels
        {
            get;
            set;
        }

        public Dictionary<string, object> Variables
        {
            get;
            set;
        } = new();

        public string? Email { get; set; }

        public string? Mobile { get; set; }

        public string? DeviceToken { get; set; }
    }
}
