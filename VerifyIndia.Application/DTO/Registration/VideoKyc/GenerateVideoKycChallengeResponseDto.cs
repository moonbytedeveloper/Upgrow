using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration.VideoKyc
{
    public sealed class GenerateVideoKycChallengeResponseDto
    {
        public RegistrationChallengeDto Challenge { get; set; }
            = new();
    }
}
