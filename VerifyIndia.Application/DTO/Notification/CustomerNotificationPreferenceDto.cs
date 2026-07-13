using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Notification
{
    public class CustomerNotificationPreferenceDto
    {
        public string? UUID { get; set; }
        public string CustomerUUID { get; set; } = string.Empty;
        public bool IsEmailEnabled { get; set; }
        public bool IsPushNtfEnabled { get; set; }
        public bool IsNtfSoundEnabled { get; set; }
        public string NotificationSoundUUID { get; set; }
        public bool IsSmsEnabled { get; set; }
        public bool IsWhatsAppEnabled { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
