using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class OTPVerifyDto
    {
        public string? client_id { get; set; }
        public string? otp { get; set; }
        //public string? AuthorisedKey { get; set; }
        //public string? Token { get; set; }
        //public string? ContentType { get; set; }
    }
}
