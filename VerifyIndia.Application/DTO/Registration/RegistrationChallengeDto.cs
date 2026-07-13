using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration
{
    public sealed class RegistrationChallengeDto
    {
        public string? ChallengeText { get; set; }

        public int? ChallengeExpiresInSeconds { get; set; }

        public DateTimeOffset? ChallengeExpiresAtUtc { get; set; }
    }
}
