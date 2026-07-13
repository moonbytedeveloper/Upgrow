using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class VehicalChallanV1Dto
    {
        public string refid { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string vehicle_id { get; set; }
        public string?engine_number { get; set; }
        public string? chassis { get; set; }

    }
}
