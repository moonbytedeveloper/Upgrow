using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class PennyDropVerifyRequest : BaseDto
    {

        [Required(ErrorMessage = "Required!")]
        public string ifsc { get; set; } 

        [Required(ErrorMessage = "Required!")]
        public string account_number { get; set; } 
    }
}
