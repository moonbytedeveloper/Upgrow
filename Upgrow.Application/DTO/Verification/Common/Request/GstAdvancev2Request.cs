using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class GstAdvancev2Request : BaseDto
    {
        [Required(ErrorMessage = "Required!")]
        public string? gst_number { get; set; }
    }
}
