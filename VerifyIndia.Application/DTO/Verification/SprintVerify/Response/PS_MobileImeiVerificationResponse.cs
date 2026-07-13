using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify.Response
{
    public class PS_MobileImeiVerificationResponse
    {
        public string? imei { get; set; }
        public bool? is_valid { get; set; }
        public string? brand_name { get; set; }
        public string? model_name { get; set; }
    }
}
