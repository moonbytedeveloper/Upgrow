using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration.VideoKyc
{
    public sealed class FaceDetectionResultDto
    {
        public bool FaceDetected { get; set; }

        public decimal FacePercentage { get; set; }

        public int TotalFrames { get; set; }

        public int FaceFrames { get; set; }
    }
}
