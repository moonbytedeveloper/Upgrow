using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Enums;
using static Upgrow.Application.Constants;

namespace Upgrow.Application.Constant
{
    public static class NotificationRouting
    {
        public static readonly IReadOnlyDictionary<string, NotificationChannel[]> Routes =
            new Dictionary<string, NotificationChannel[]>
            {
                [NotificationEvents.LoginOtp] =
                [
                    NotificationChannel.Sms,
                    NotificationChannel.WhatsApp
                ],

                [NotificationEvents.EmailVerification] =
                [
                    NotificationChannel.Email,                    
                    
                ],
                [NotificationEvents.MobileVerification] =
                [
                    NotificationChannel.Sms,

                ],
                [NotificationEvents.ConsentLink] =
                [
                    NotificationChannel.WhatsApp,

                ],
                [NotificationEvents.ResetPassword] =
                [
                    NotificationChannel.Email,

                ],
            };


    }
}
