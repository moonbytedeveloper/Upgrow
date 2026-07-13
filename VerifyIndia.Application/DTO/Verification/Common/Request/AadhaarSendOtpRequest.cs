using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class AadhaarSendOtpRequest
    {
        [Required(ErrorMessage = "Required!")]
        public string aadhaar_number { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string consent { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string reason { get; set; } 
    }
}
