using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Sandbox
{
    public class SB_PanVerifyDto
    {
       
        [Required(ErrorMessage = "Required!")]
        public string pan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required!")]
        public string name_as_per_pan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required!")]
        public DateOnly date_of_birth { get; set; } 

        [Required(ErrorMessage = "Required!")]
        public string consent { get; set; } = "Y";

        [Required(ErrorMessage = "Required!")]
        public string reason { get; set; } 
    }
}
