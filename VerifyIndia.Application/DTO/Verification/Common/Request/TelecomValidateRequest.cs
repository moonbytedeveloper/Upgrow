using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class TelecomValidateRequest
    {
        [Required(ErrorMessage = "Id_number is required")]
        public string id_number { get; set; }
    }
}
