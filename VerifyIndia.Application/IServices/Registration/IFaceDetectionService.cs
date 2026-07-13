using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Registration.VideoKyc;

namespace VerifyIndia.Application.IServices.Registration
{
    public interface IFaceDetectionService
    {
        Task<FaceDetectionResultDto>
            DetectFaceAsync(
                string videoPath);
    }
}
