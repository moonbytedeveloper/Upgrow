using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class RtoInformationDto
    {
        public string refid { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string rto_code { get; set; }
    }
}
