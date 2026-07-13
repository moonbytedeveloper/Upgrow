using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class CardValidationDto
    {
        [Required(ErrorMessage = "Required!")]
        public string? card_num { get; set; }
    }
}
