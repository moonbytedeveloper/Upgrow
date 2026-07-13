using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Registration.VideoKyc;
using VerifyIndia.Domain.Entities.Registration;

namespace VerifyIndia.Application.IServices.Registration
{
    public interface IVideoKycVerificationService
    {
        Task<VideoKycVerificationResultDto>
            VerifyAsync(
                CustomerVideoKyc challenge,
                string videoUrl);
    }
}
