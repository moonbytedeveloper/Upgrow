using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class TelecomValidateDto
    {
        [Required(ErrorMessage = "Id_number is required")]
        public string id_number { get; set; }
    }
    public class TelecomDetailDto
    {
        public string refid { get; set; }
        [Required(ErrorMessage = "ClientId is required")]
        public string client_id { get; set; }
        [Required(ErrorMessage = "otp is required")]
        public string otp { get; set; }
    }
}