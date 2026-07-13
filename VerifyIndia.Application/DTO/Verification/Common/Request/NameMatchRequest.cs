using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class NameMatchRequest
    {
        [Required(ErrorMessage = "Required!")]
        public string name_1 { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string name_2 { get; set; }
    }
}
