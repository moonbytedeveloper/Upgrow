using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Response
{
    public class PanVerifyResponse
    {
       
        public string? pan_number { get; set; }
        public string? full_name { get; set; }
        public string? gender { get; set; }
        public string? dob { get; set; }
        public string? category { get; set; }

    }
}
