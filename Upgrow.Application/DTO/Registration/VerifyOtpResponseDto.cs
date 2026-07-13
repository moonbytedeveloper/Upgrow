using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Common;

namespace Upgrow.Application.DTO.Registration
{
    public sealed class VerifyOtpResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime AccessTokenExpiresAtUtc { get; set; }

        public DateTime RefreshTokenExpiresAtUtc { get; set; }

        public string CurrentStep { get; set; } = string.Empty;

        public string CustomerUUID { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;

        public RegistrationStateDto RegistrationState { get; set; } = new();

        public OtpPolicyDto OtpPolicy { get; set; } = new();
    }
}
