using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Transaction
{
    public sealed class ContinueVerificationRequest
    {
        //public JsonElement Payload { get; init; }

        [Required]
        public string ClientId { get; set; }
        = string.Empty;

        [Required]
        public string Otp { get; set; }
            = string.Empty;
    }
}
