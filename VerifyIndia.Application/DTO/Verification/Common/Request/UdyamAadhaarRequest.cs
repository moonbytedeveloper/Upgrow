using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class UdyamAadhaarRequest : BaseDto
    {
       // public string refid { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string id_number { get; set; }

    }
}
