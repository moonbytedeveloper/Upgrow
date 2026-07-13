using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class LEIVerifyRequest : BaseDto
    {
        //public string refid { get; set; }
        public string? id_number { get; set; }
        //public string? Token { get; set; }
        //public string? Authorisedkey { get; set; }
        //public string? ContentType { get; set; }
    }
}
