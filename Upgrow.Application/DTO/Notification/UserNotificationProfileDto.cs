using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Notification;

public sealed class UserNotificationProfileDto
{
    public string UserId { get; set; }
        = string.Empty;

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public bool EmailEnabled { get; set; } = true;

    public bool SmsEnabled { get; set; } = true;

    public bool WhatsAppEnabled { get; set; } = true;

    public bool PushEnabled { get; set; } = true;

    public List<string> DeviceTokens
    {
        get;
        set;
    } = [];
}