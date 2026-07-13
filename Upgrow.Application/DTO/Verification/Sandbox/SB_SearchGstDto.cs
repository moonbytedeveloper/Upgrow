using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Sandbox
{
    public class SB_SearchGstDto
    {
        [Required(ErrorMessage = "Required!")]
        public string pan { get; set; } = string.Empty;
    }
}
