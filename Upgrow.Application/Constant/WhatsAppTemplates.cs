using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Constant;

public static class WhatsAppTemplates
{
    public static readonly IReadOnlyDictionary<string,string> Templates =
        new Dictionary<string, string>
        {
            [NotificationEvents.LoginOtp] = "ai_login_otp",
            [NotificationEvents.ConsentLink] = "ai_consent",
        };
}
