using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class TelecomDetailRequest : BaseDto
    {
        //public string refid { get; set; }
        [Required(ErrorMessage = "ClientId is required")]
        public string client_id { get; set; }
        [Required(ErrorMessage = "otp is required")]
        public string otp { get; set; }
    }
}
