using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class PanAadharLinkRequest
    {
        [Required(ErrorMessage = "Required!")]
        public string pan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required!")]
        public string aadhaar_number { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required!")]
        public string consent { get; set; } = "Y";

        [Required(ErrorMessage = "Required!")]
        public string reason { get; set; } = "Testing API";
    }
}
