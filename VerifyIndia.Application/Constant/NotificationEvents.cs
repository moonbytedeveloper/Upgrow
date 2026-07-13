using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Constant
{
    public static class NotificationEvents
    {
        // Caution - These event codes are used as keys in the database and should not be changed lightly.
        public const string LoginOtp = "LOGIN_OTP";
        public const string EmailVerification = "EMAIL_VERIFICATION";
        public const string MobileVerification = "MOBILE_VERIFICATION";
        public const string ConsentLink = "CONSENT_LINK";
        public const string ResetPassword = "RESET_PASSWORD";
    }
}
