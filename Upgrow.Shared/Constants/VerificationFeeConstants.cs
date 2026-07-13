using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Shared.Constants
{
    public static class VerificationFeeConstants
    {
        public const string AADHAAR = nameof(AADHAAR);
        public const string CONSENT_FEE = nameof(CONSENT_FEE);
        public const string BUSINESS_VERIFICATION_FEE = nameof(BUSINESS_VERIFICATION_FEE);

        public static readonly IReadOnlyList<(string Value, string Text)> Types =
        [
            (AADHAAR, "Aadhaar"),
            (CONSENT_FEE, "Consent Fee"),
            (BUSINESS_VERIFICATION_FEE, "Business Verification Fee")
        ];
    }
}
