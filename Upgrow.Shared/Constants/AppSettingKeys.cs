using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Shared.Constants
{
    public static class AppSettingKeys
    {
        public const string OTP_MOBILE_EXPIRY_SECONDS =
            "OTP_MOBILE_EXPIRY_SECONDS";

        public const string OTP_MOBILE_MAX_RESEND_COUNT =
            "OTP_MOBILE_MAX_RESEND_COUNT";

        public const string OTP_MOBILE_MAX_VERIFY_ATTEMPTS =
            "OTP_MOBILE_MAX_VERIFY_ATTEMPTS";

        public const string OTP_MOBILE_RESEND_COOLDOWN_SECONDS =
            "OTP_MOBILE_RESEND_COOLDOWN_SECONDS";

        public const string OTP_MOBILE_LOCKOUT_SECONDS =
            "OTP_MOBILE_LOCKOUT_SECONDS";

        public const string OTP_AADHAAR_EXPIRY_SECONDS =
            "OTP_AADHAAR_EXPIRY_SECONDS";

        public const string OTP_AADHAAR_MAX_RESEND_COUNT =
            "OTP_AADHAAR_MAX_RESEND_COUNT";

        public const string OTP_AADHAAR_MAX_VERIFY_ATTEMPTS =
            "OTP_AADHAAR_MAX_VERIFY_ATTEMPTS";

        public const string OTP_AADHAAR_RESEND_COOLDOWN_SECONDS =
            "OTP_AADHAAR_RESEND_COOLDOWN_SECONDS";

        public const string OTP_AADHAAR_LOCKOUT_SECONDS =
            "OTP_AADHAAR_LOCKOUT_SECONDS";

        public const string VIDEO_KYC_CHALLENGE_TEXT =
            "VIDEO_KYC_CHALLENGE_TEXT";

        public const string VIDEO_KYC_CHALLENGE_EXPIRY_SECONDS =
            "VIDEO_KYC_CHALLENGE_EXPIRY_SECONDS";

        public const string VIDEO_KYC_RECORDING_DURATION_SECONDS =
            "VIDEO_KYC_RECORDING_DURATION_SECONDS";

        public const string VIDEO_KYC_MIN_MATCH_PERCENTAGE =
            "VIDEO_KYC_MIN_MATCH_PERCENTAGE";

        public const string CONSENT_LINK_EXPIRY_SECONDS =
            "CONSENT_LINK_EXPIRY_SECONDS";

        public const string OTP_EMAIL_RESEND_COOLDOWN_SECONDS =
            "OTP_EMAIL_RESEND_COOLDOWN_SECONDS";

        public const string OTP_EMAIL_EXPIRY_SECONDS =
            "OTP_EMAIL_EXPIRY_SECONDS";


        public const string FORGOT_PASSWORD_EXPIRY_SECONDS =
            "FORGOT_PASSWORD_EXPIRY_SECONDS";

        public static readonly List<string> All = new()
        {
            OTP_MOBILE_EXPIRY_SECONDS,
            OTP_MOBILE_MAX_RESEND_COUNT,
            OTP_MOBILE_MAX_VERIFY_ATTEMPTS,
            OTP_MOBILE_RESEND_COOLDOWN_SECONDS,
            OTP_MOBILE_LOCKOUT_SECONDS,

            OTP_AADHAAR_EXPIRY_SECONDS,
            OTP_AADHAAR_MAX_RESEND_COUNT,
            OTP_AADHAAR_MAX_VERIFY_ATTEMPTS,
            OTP_AADHAAR_RESEND_COOLDOWN_SECONDS,
            OTP_AADHAAR_LOCKOUT_SECONDS,

            VIDEO_KYC_CHALLENGE_TEXT,
            VIDEO_KYC_CHALLENGE_EXPIRY_SECONDS,
            VIDEO_KYC_RECORDING_DURATION_SECONDS,
            VIDEO_KYC_MIN_MATCH_PERCENTAGE,

            CONSENT_LINK_EXPIRY_SECONDS,

            OTP_EMAIL_RESEND_COOLDOWN_SECONDS,
            OTP_EMAIL_EXPIRY_SECONDS,
            FORGOT_PASSWORD_EXPIRY_SECONDS
        };
    }
}
