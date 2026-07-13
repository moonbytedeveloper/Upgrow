using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class ItrSubmitOtpRequest
    {
        [Required(ErrorMessage = "Required!")]
        public string client_id { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string otp { get; set; }
    }
}
