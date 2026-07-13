using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class GstAdvanceV2Dto
    {
        public string refid { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string gst_number { get; set; }

    }
}
