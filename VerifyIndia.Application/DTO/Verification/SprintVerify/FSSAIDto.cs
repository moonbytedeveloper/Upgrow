using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class FSSAIDto
    {
        public string refid { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string fssai_number { get; set; }
    }
}
