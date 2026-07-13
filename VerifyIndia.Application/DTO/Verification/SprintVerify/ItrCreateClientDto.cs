using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class ItrCreateClientDto
    {
        [Required(ErrorMessage = "Required!")]
        public string username { get; set; }
        public string? password { get; set; }
    }
}
