using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class MCARequest : BaseDto
    {
        //public string refid { get; set; }
        public string? id_number { get; set; }
    }
}
