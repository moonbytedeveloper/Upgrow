using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration.VideoKyc
{
    public sealed class SubmitVideoKycRequestDto
    {
        public string ChallengeText { get; set; }
            = string.Empty;

        public string SpokenText { get; set; }
            = string.Empty;

        public decimal MatchPercentage { get; set; }

        public bool FaceDetected { get; set; }

        public string? LatLong { get; set; }

        public string? Platform { get; set; }

        public IFormFile Video { get; set; }
            = default!;
    }
}
