using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class shopestablishmentDto
    {
        public string refid { get; set; }
        public string? state_code { get; set; }

        public string? shop_number { get; set; }
    }
}
