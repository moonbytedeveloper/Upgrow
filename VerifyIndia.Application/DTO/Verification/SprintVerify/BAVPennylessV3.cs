using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class BAVPennylessV3
    {
        public string refid { get; set; }
        public string? account_number { get; set; }
        public string? ifsc_code { get; set; }
    }
}
