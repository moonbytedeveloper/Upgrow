using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Transaction
{
    public class ApproveConsentRequestDto
    {
        /// <summary>
        /// ClientId / RequestId received from Send Aadhaar OTP API.
        /// </summary>
        public string ClientId { get; set; }
            = string.Empty;

        /// <summary>
        /// Aadhaar OTP.
        /// </summary>
        public string AadhaarOtp { get; set; }
            = string.Empty;

        /// <summary>
        /// Consent giver name.
        /// </summary>
        public string SubmitterName { get; set; }
            = string.Empty;
    }

    public class RejectConsentRequestDto
    {
        public string SubmitterName { get; set; }
            = string.Empty;
    }
}
