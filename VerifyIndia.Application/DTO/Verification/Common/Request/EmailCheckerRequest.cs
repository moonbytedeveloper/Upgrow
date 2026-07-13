using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class EmailCheckerRequest : BaseDto
    {
        //public string refid { get; set; }
        public string? email { get; set; }
    }
}
