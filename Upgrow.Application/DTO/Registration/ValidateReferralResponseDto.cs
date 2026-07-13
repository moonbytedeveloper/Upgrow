using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration
{
    public sealed class ValidateReferralResponseDto
    {
        public bool IsValid { get; set; }

        public string? AgentName { get; set; }

        public string? AgentUUID { get; set; }
    }
}
