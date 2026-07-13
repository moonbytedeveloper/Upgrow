using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class DinVerificationRequestDto
    {
        [Required(ErrorMessage = "Required!")]
        public string din_number { get; set; }

        public string refid { get; set; }
    }
}
