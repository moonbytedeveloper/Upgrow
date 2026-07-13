using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class DinVerificationRequest : BaseDto
    {
        [Required(ErrorMessage = "Required!")]
        public string din_number { get; set; }

        //public string refid { get; set; }
    }
}
