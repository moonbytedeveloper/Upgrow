using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class UanBasicV1Dto
    {
        public string refid { get; set; }
        public string? employee_name { get; set; }
        public string? employer_name { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string type { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string mobile { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string pan_number { get; set; }
        public string? uan { get; set; }
    }
}
