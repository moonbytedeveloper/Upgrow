using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Transaction
{
    public class SubmitConsentRequest
    {
        public string AadhaarOtp { get; set; }
            = string.Empty;

        public string SubmitterName { get; set; }
        = string.Empty;
    }
}
