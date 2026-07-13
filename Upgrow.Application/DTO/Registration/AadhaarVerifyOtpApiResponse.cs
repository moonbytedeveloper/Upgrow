using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration
{
    public class AadhaarVerifyOtpApiResponse
    {
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
            = string.Empty;

        [JsonPropertyName("aadhaar_number")]
        public string AadhaarNumber { get; set; }
            = string.Empty;

        [JsonPropertyName("reference_id")]
        public string ReferenceId { get; set; }
            = string.Empty;
    }
}
