using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify.Response
{
    public class PS_IfscVerifyResponse
    {
        public string? ifsc { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
        public string? branch { get; set; }
        public string? micr { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? district { get; set; }
        public string? contact { get; set; }
        public bool? upi { get; set; }     
        public bool? imps { get; set; }    
        public bool? neft { get; set; }    
        public bool? rtgs { get; set; }
        public string? swift { get; set; }
        public string? logo { get; set; }

    }
}
