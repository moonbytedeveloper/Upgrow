using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class MobileIntelligenceRequest : BaseDto
    {
        //public string refid { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string phone { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string first_name { get; set; }
        public string? last_name { get; set; }
        public string? pan { get; set; }
    }
}
