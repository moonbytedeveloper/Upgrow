using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class ItrForgetPasswordDto
    {
       [Required(ErrorMessage = "Required!")]
        public string client_id { get; set; }
        public string? password { get; set; }
    }
}
