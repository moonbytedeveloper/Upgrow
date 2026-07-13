using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class PincodeInfoRequest : BaseDto
    {
        //public string refid { get; set; }

        [Required(ErrorMessage = "Required!")]
        public int pincode { get; set; }
    }
}
