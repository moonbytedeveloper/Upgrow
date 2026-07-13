using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Common;

namespace Upgrow.Application.DTO.Registration
{
    public sealed class SendOtpResponseDto
    {
        public string CustomerUUID { get; set; } = string.Empty;

        public OtpPolicyDto OtpPolicy { get; set; } = new();
    }
}
