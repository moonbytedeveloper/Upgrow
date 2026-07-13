using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Registration.VideoKyc;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Application.IServices.Registration
{
    public interface IVideoKycVerificationService
    {
        Task<VideoKycVerificationResultDto>
            VerifyAsync(
                CustomerVideoKyc challenge,
                string videoUrl);
    }
}
