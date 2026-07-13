using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Response
{
    public class PennyLessVerifyResponse
    {
        public string? beneficiary_name { get; set; }
        public string? beneficiary_account { get; set; }
        public string? beneficiary_ifsc { get; set; }
        public string? bank_name { get; set; }
        public string? branch_name { get; set; }
    }
}
