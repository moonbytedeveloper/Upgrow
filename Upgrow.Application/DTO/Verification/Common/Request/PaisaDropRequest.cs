using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class PaisaDropRequest
    {
        [Required(ErrorMessage = "Required!")]
        public int account_number { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string ifsc_code { get; set; }
    }
}
