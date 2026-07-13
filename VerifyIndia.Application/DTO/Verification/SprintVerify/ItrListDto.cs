using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class ItrListDto
    {
        public string? refid { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string client_id { get; set; }
    }
}
