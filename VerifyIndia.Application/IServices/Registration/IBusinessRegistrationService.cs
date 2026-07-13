using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Registration;
using VerifyIndia.Application.DTO.Registration.Business;

namespace VerifyIndia.Application.IServices.Registration
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
