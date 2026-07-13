using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NotificationChannel
    {
        Email = 1,
        Sms = 2,
        WhatsApp = 3,
        Push = 4
    }
}
