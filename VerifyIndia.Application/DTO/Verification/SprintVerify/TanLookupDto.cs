using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class TanLookupDto
    {
        public string refid { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string company_name { get; set; }
    }
}
