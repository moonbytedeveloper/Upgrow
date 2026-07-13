using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class VehicalChallanV2Dto
    {
        public string refid { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string rc_number { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string chassis_number { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string engine_number { get; set; }
        //public string? Token { get; set; }
        //public string? Authorisedkey { get; set; }

    }
}
