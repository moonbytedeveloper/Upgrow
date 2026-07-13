using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class shopestablishmentRequest : BaseDto
    {
        //public string refid { get; set; }
        public string? state_code { get; set; }

        public string? shop_number { get; set; }
    }
}
