using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class ITRCompliancecheckRequest :BaseDto
    {

        [Required(ErrorMessage = "Required!")]
        public string pan_number { get; set; }
    }
}
