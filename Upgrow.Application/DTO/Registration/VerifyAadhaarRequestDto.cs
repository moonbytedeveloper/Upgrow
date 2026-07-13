using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration
{
    public sealed class VerifyAadhaarRequestDto
    {
        public string AadhaarNumber { get; set; }
            = string.Empty;

        public string AccountType { get; set; }
        = string.Empty;
    }
}
