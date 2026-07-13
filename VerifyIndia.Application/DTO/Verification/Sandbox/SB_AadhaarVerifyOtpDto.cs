using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Sandbox
{
    public class SB_AadhaarVerifyOtpDto
    {
        [Required(ErrorMessage = "Required!")]
        public string reference_id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required!")]
        public string otp { get; set; } = string.Empty;
    }
}
