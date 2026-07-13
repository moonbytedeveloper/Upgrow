using AuthenticateIndia.Shared.Constants;
using AuthenticateIndia.Shared.Constants.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Registration;
using Upgrow.Application.DTO.Registration.Business;
using Upgrow.Application.Interfaces.Registration;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Payment;
using Upgrow.Application.IServices.Registration;
using Upgrow.Domain.Entities.Registration;
using Upgrow.Domain.IRepositories.Registration;

namespace Upgrow.Application.Services.Registration
{
    public class BusinessRegistrationService : IBusinessRegistrationService
    {
        private readonly IRazorpayService
            _razorpayService;

        private readonly ICustomerBusinessSessionRepository
            _businessSessionRepository;

        private readonly ICustomerBusinessRepository
            _customerOrganizationRepository;

        private readonly IMasterCustomerService
            _customerService;

        private readonly IMasterBusinessTypeRepository
            _masterBusinessTypeRepository;

        private readonly IVerificationFeeRepository 
            _verificationFeeRepository;

        public BusinessRegistrationService(
            ICustomerBusinessSessionRepository businessSessionRepository,
            IRazorpayService razorpayService,
            ICustomerBusinessRepository customerOrganizationRepository,
            IMasterCustomerService masterCustomerService,
            IVerificationFeeRepository verificationFeeRepository,
            IMasterBusinessTypeRepository masterBusinessTypeRepository
            )
        {
            _businessSessionRepository =
                businessSessionRepository;

            _razorpayService =
                razorpayService;

            _customerOrganizationRepository =
                customerOrganizationRepository;

            _customerService =
                masterCustomerService;

            _verificationFeeRepository =
                verificationFeeRepository;

            _masterBusinessTypeRepository =
                masterBusinessTypeRepository;
        }

        private static void ValidateCurrentStep(
            string currentStep,
            string expectedStep)
        {
            if (!string.Equals(
                currentStep,
                expectedStep,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Expected step '{expectedStep}' but customer is at '{currentStep}'.");
            }
        }

        private CustomerOrganization
    CreateOrganization(
        string customerUuid,
        CreateBusinessOrderRequestDto request)
        {
            return new CustomerOrganization
            {
                UUID =
                    Utils.GetUUID(),

                CustomerUUID =
                    customerUuid,

                BusinessTypeUUID =
                    request.BusinessTypeUUID,

                BusinessRegistrationNumber =
                    request.BusinessRegistrationNumber,

                VerificationDocumentUUID =
                    request.VerificationDocumentUUID,

                VerificationDocumentNumber =
                    request.VerificationDocumentNumber,

                PAN =
                    request.PAN,

                GSTIN =
                    request.GSTIN,

                VerificationStatus =
                    OrganizationVerificationStatus.Pending,

                IsActive =
                    true,

                CreatedAt =
                    DateTimeOffset.UtcNow
            };
        }

        private static void
    UpdateOrganization(
        CustomerOrganization organization,
        CreateBusinessOrderRequestDto request)
        {
            organization.BusinessTypeUUID =
                request.BusinessTypeUUID;

            organization.BusinessRegistrationNumber =
                request.BusinessRegistrationNumber;

            organization.VerificationDocumentUUID =
                request.VerificationDocumentUUID;

            organization.VerificationDocumentNumber =
                request.VerificationDocumentNumber;

            organization.PAN =
                request.PAN;

            organization.GSTIN =
                request.GSTIN;

            organization.VerificationStatus =
                OrganizationVerificationStatus.Pending;

            organization.UpdatedAt =
                DateTimeOffset.UtcNow;
        }

        private static void
    ValidateBusinessRequest(
        string businessTypeCode,
        CreateBusinessOrderRequestDto request)
        {
            switch (businessTypeCode)
            {
                case BusinessTypeCodes.PRIVATE_LIMITED:

                case BusinessTypeCodes.PUBLIC_LIMITED:

                    if (string.IsNullOrWhiteSpace(
                            request.BusinessRegistrationNumber))
                    {
                        throw new Exception(
                            "CIN is required.");
                    }

                    break;

                case BusinessTypeCodes.LLP:

                    if (string.IsNullOrWhiteSpace(
                            request.BusinessRegistrationNumber))
                    {
                        throw new Exception(
                            "LLPIN is required.");
                    }

                    break;

                case BusinessTypeCodes.PROPRIETORSHIP:

                    if (string.IsNullOrWhiteSpace(
                            request.VerificationDocumentUUID))
                    {
                        throw new Exception(
                            "Verification document is required.");
                    }

                    if (string.IsNullOrWhiteSpace(
                            request.VerificationDocumentNumber))
                    {
                        throw new Exception(
                            "Document number is required.");
                    }

                    break;

                case BusinessTypeCodes.PARTNERSHIP:

                    if (string.IsNullOrWhiteSpace(
                            request.PAN))
                    {
                        throw new Exception(
                            "PAN is required.");
                    }

                    if (string.IsNullOrWhiteSpace(
                            request.VerificationDocumentUUID))
                    {
                        throw new Exception(
                            "Verification document is required.");
                    }

                    if (string.IsNullOrWhiteSpace(
                            request.VerificationDocumentNumber))
                    {
                        throw new Exception(
                            "Document number is required.");
                    }

                    break;

                default:

                    throw new Exception(
                        "Unsupported business type.");
            }
        }

        public async Task<CreateBusinessOrderResponseDto>
    CreateBusinessOrderAsync(
        string customerUuid,
        CreateBusinessOrderRequestDto request)
        {
            var customer =
                await _customerService
                    .GetCustomerByUUID(
                        customerUuid);

            if (customer == null)
            {
                throw new Exception(
                    "Customer not found.");
            }

            ValidateCurrentStep(
                customer.CurrentStep,
                RegistrationSteps.SELECT_ACCOUNT_TYPE);

            var businessType =
                await _masterBusinessTypeRepository
                    .GetByUUIDAsync(
                        request.BusinessTypeUUID);

            if (businessType == null)
            {
                throw new Exception(
                    "Invalid business type.");
            }

            ValidateBusinessRequest(
                businessType.Code,
                request);

            var organization =
                await _customerOrganizationRepository
                    .GetByCustomerUUIDAsync(
                        customerUuid);

            if (organization == null)
            {
                organization =
                    CreateOrganization(
                        customerUuid,
                        request);
            }
            else
            {
                UpdateOrganization(
                    organization,
                    request);
            }

            if (organization.Id == 0)
            {
                await _customerOrganizationRepository
                    .AddAsync(
                        organization);
            }
            else
            {
                await _customerOrganizationRepository
                    .UpdateAsync(
                        organization);
            }

            var fee = await _verificationFeeRepository
                .GetByTypeAsync(VerificationFeeConstants.BUSINESS_VERIFICATION_FEE);

            if (fee == null)
            {
                throw new Exception(
                    "Business verification fee is not configured.");
            }

            var sessionUuid =
                Utils.GetUUID();

            var razorpayOrder =
                await _razorpayService
                    .CreateOrderAsync(
                        fee.Amount,
                        sessionUuid);

            await _businessSessionRepository
                .DeactivateCustomerSessionsAsync(
                    customerUuid);

            var session =
                new CustomerBusinessSession
                {
                    UUID =
                        sessionUuid,

                    CustomerUUID =
                        customerUuid,

                    CustomerOrganizationUUID =
                        organization.UUID,

                    RazorpayOrderId =
                        razorpayOrder.OrderId,

                    Amount =
                        fee.Amount,

                    CreatedAt =
                        DateTimeOffset.UtcNow,

                    IsActive =
                        true
                };

            await _businessSessionRepository
                .AddAsync(
                    session);

            return new CreateBusinessOrderResponseDto
            {
                SessionUUID =
                    sessionUuid,

                OrderId =
                    razorpayOrder.OrderId,

                Amount =
                    fee.Amount,

                RazorpayKeyId =
                    razorpayOrder.KeyId
            };
        }

        public async Task<BusinessMetadataResponseDto> GetBusinessMetadataAsync()
        {
            var businessTypes =
                await _masterBusinessTypeRepository
                    .GetAllActiveAsync();

            var fee =
                await _verificationFeeRepository
                    .GetByTypeAsync(VerificationFeeConstants.BUSINESS_VERIFICATION_FEE);

            if (fee == null)
            {
                throw new Exception(
                    "Business verification fee not found.");
            }

            return new BusinessMetadataResponseDto
            {
                BusinessVerificationFee =
                    fee?.Amount ?? 0,

                BusinessTypes =
                    businessTypes
                        .OrderBy(x => x.DisplayOrder)
                        .Select(x =>
                            new BusinessTypeDto
                            {
                                UUID =
                                    x.UUID,

                                Name =
                                    x.Name,

                                DisplayOrder =
                                    x.DisplayOrder
                            })
                        .ToList()
            };
        }

        public async Task<RegistrationStateDto> CompleteBusinessVerificationAsync(
            string customerUuid,
            CompleteBusinessVerificationRequestDto request)
        {
            throw new NotImplementedException();
           /* ValidateSignature(...);

            var session = ...

    var organization = ...

    VerifyPayment(...);

            switch (organization.BusinessType.Code)
            {
                case BusinessTypeCodes.PRIVATE_LIMITED:

                case BusinessTypeCodes.PUBLIC_LIMITED:

                    await VerifyCompanyAsync(...);

                    break;

                case BusinessTypeCodes.LLP:

                    await VerifyLlpAsync(...);

                    break;

                case BusinessTypeCodes.PROPRIETORSHIP:

                    await VerifyProprietorshipAsync(...);

                    break;

                case BusinessTypeCodes.PARTNERSHIP:

                    await VerifyPartnershipAsync(...);

                    break;
            }

            return await _registrationStateBuilder
                .BuildAsync(customerUuid);*/
        }

    }
}
