using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Registration.VideoKyc;

namespace Upgrow.Application.IServices.Registration
{
    public interface IFaceDetectionService
    {
        Task<FaceDetectionResultDto>
            DetectFaceAsync(
                string videoPath);
    }
}
