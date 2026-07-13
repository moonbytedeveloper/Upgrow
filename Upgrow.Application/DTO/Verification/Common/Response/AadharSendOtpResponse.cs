using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Response
{
    public class AadharSendOtpResponse
    {
        public string? client_id { get; set; }
        public bool? otp_sent { get; set; }
        public bool? if_number { get; set; }
        public bool? valid_aadhaar { get; set; }
        public string? status { get; set; }
    }
}
