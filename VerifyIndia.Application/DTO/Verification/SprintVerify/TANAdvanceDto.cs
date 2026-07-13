using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class TANAdvanceDto
    {
        public string refid { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string tan_number { get; set; }

    }
}
