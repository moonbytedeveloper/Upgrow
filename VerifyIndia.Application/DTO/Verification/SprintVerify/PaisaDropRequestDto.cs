using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class PaisaDropRequestDto
    {
        [Required(ErrorMessage = "Required!")]
        public int? account_number { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string ifsc_code { get; set; }
    }
}
