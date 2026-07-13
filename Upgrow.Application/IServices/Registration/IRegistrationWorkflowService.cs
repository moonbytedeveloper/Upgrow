using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Common.Results;
using Upgrow.Application.DTO.Registration;
using Upgrow.Application.DTO.Registration.AadhaarVerification;
using Upgrow.Application.DTO.Registration.VideoKyc;
using static Upgrow.Application.Services.Registration.RegistrationWorkflowService;

namespace Upgrow.Application.IServices.Registration
{
    public interface IRegistrationWorkflowService
    {
        Task<GenerateVideoKycChallengeResponseDto> GenerateVideoKycChallengeAsync(
            string customerUuid);

        Task<ValidateReferralResponseDto> ValidateReferralAsync(
            string referralCode);

        Task<LocationLookupResponseDto> GetLocationAsync(
            decimal latitude,
            decimal longitude);
        Task<CreateAadhaarOrderResponseDto> CreateAadhaarOrderAsync(
            string customerUuid,
            VerifyAadhaarRequestDto request);

        Task<AadhaarPaymentSuccessResponseDto> AadhaarPaymentSuccessAsync(
            string customerUuid,
            AadhaarPaymentSuccessRequestDto request);

        Task<VerifyAadhaarOtpWorkflowResponseDto> VerifyAadhaarOtpAsync(
            string customerUuid,
            VerifyAadhaarOtpRequestDto request);

        Task<SendOtpResponseDto> SendOtpAsync(
            SendOtpRequestDto request);

        Task<VerifyOtpResultDto> VerifyOtpAsync(
            VerifyOtpRequestDto request);

        Task<RegistrationStateDto> GetRegistrationStateAsync(
            string customerUuid);

        Task<RegistrationStateDto> SaveBasicInfoAsync(
            string customerUuid,
            SaveBasicInfoRequestDto request);

        Task<RegistrationStateDto> UploadVideoKycAsync(
            string customerUuid,
            UploadVideoKycRequestDto request);

        Task<AcceptTermsResponseDto> AcceptTermsAsync(
            string customerUuid,
            AcceptTermsRequestDto request);

        Task<RegistrationStateDto> CompleteTermsAsync(
            string customerUuid);

        Task<RegistrationStateDto> SaveReferralAsync(
            string customerUuid,
            SaveReferralRequestDto request);

        Task<SendAadhaarOtpResponseDto> ResendAadhaarOtpAsync(
            string customerUuid,
            ResendAadhaarOtpRequestDto request);

        Task<RegistrationStateDto> AcceptDosDontsAsync(
            string customerUuid,
            AcceptDosDontsRequestDto request);
    }
}
