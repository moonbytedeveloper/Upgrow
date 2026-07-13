using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class IFSCLookupRequestDto
    {
        public string ifsc { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string refid { get; set; }
    }
}
