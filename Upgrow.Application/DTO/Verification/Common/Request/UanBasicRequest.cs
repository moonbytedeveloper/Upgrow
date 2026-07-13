using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class UanBasicRequest : BaseDto
    {
       // public string refid { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string type { get; set; }
        public string? method { get; set; }
        public string? employee_name { get; set; }
        public string? employer_name { get; set; }
        public string? pan_number { get; set; }
        public string? mobile { get; set; }
        public string? dob { get; set; }
        public string? uan { get; set; }
    }
}
