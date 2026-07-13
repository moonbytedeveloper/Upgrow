using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Registration;
using Upgrow.Application.DTO.Registration.Business;

namespace Upgrow.Application.IServices.Registration
{
    public interface IBusinessRegistrationService
    {
        Task<BusinessMetadataResponseDto> GetBusinessMetadataAsync();

        Task<CreateBusinessOrderResponseDto> CreateBusinessOrderAsync(
            string customerUuid,
            CreateBusinessOrderRequestDto request);

        Task<RegistrationStateDto> CompleteBusinessVerificationAsync(
            string customerUuid,
            CompleteBusinessVerificationRequestDto request);
    }
}
