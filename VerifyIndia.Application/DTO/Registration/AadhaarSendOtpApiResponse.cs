using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration
{
    public class AadhaarSendOtpApiResponse
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
            = string.Empty;

        [JsonPropertyName("otp_sent")]
        public bool OtpSent { get; set; }
    }
}
