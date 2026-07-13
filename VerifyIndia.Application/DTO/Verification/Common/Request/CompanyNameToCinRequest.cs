using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class CompanyNameToCinRequest : BaseDto
    {
        //public string refid { get; set; }
        [Required(ErrorMessage = "CompanyName is required")]
        public string CompanyName { get; set; }
    }
}
