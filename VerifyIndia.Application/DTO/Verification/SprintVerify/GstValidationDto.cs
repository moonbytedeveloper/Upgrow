using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class GstValidationDto
    {
        public string refid { get; set; }
        public string? id_number { get; set; }
        public string? filling_status { get; set; }
    }
}
