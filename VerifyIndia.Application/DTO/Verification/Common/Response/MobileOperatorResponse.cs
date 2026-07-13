using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Response
{
    public class MobileOperatorResponse
    {
        public string? mobile { get; set; }

        [JsonPropertyName("operator")]
        public string? Operator { get; set; }
        public bool? postpaid { get; set; }
    }
}
