//using Upgrow.Application.Constant;
using AuthenticateIndia.Shared.Constants;
using AuthenticateIndia.Shared.Constants.Registration;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection.Metadata;
using Upgrow.Application.DTO.Customer;
using Upgrow.Application.DTO.Registration;
using Upgrow.Application.DTOs.Master;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Registration;
using Upgrow.Domain.Entities.Registration;
using Upgrow.Domain.IRepositories.Master;
using Upgrow.Domain.IRepositories.Registration;
using ICustomerVideoKycRepository = Upgrow.Application.Interfaces.Registration.ICustomerVideoKycRepository;


namespace Upgrow.Application.Services.Registration
{
    public class RegistrationStateBuilder : IRegistrationStateBuilder
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IMasterDosDontsDocumentRepository _masterDosDontsDocumentRepository;
        private readonly ICustomerConsentDosDontsRepository _customerConsentDosDontsRepository;
        private readonly ICustomerAadhaarSessionRepository _aadhaarSessionRepository;
        private readonly IAppSettingService _appSettingService;
        private readonly ICustomerVideoKycRepository _customerVideoKycRepository;
        private readonly ICustomerConsentRepository _customerConsentRepository;
        private readonly IMasterCustomerService _customerService;
        private readonly IMasterStateService _stateService;
        private readonly IMasterCityService _cityService;
        private readonly IMasterIndustryService _industryService;
        private readonly IMasterPolicyService _policyService;
        private readonly IMasterDosDontsService _dosDontsService;
        private readonly IVerificationFeeRepository _verificationFeeRepository;
        private readonly ICustomerRegDocumentRepository _customerRegDocumentRepository;

        public RegistrationStateBuilder(
            IEncryptionService encryptionService,
            ICustomerRegDocumentRepository customerRegDocumentRepository,
            IMasterDosDontsDocumentRepository masterDosDontsDocumentRepository,
            ICustomerConsentDosDontsRepository customerConsentDosDontsRepository,
            ICustomerAadhaarSessionRepository customerAadhaarSessionRepository,
            IAppSettingService appSettingService,
            ICustomerVideoKycRepository customerVideoKycRepository,
            ICustomerConsentRepository customerConsentRepository,
            IVerificationFeeRepository verificationFeeRepository,
            IMasterCustomerService customerService,
            IMasterStateService stateService,
            IMasterCityService cityService,
            IMasterIndustryService industryService,
            IMasterPolicyService policyService,
            IMasterDosDontsService dosDontsService)
        {
            _encryptionService = encryptionService;
            _customerRegDocumentRepository = customerRegDocumentRepository;
            _masterDosDontsDocumentRepository = masterDosDontsDocumentRepository;
            _customerConsentDosDontsRepository = customerConsentDosDontsRepository;
            _aadhaarSessionRepository = customerAadhaarSessionRepository;
            _appSettingService = appSettingService;
            _customerVideoKycRepository = customerVideoKycRepository;
            _customerConsentRepository = customerConsentRepository;
            _verificationFeeRepository = verificationFeeRepository;
            _customerService = customerService;
            _stateService = stateService;
            _cityService = cityService;
            _industryService = industryService;
            _policyService = policyService;
            _dosDontsService = dosDontsService;
        }

        

        public async Task<RegistrationStateDto> BuildAsync(
            string customerUuid,
            bool generateKycChallenge = false)
        {
            try
            {
                var customer =
                        await _customerService.GetCustomerByUUID(
                            customerUuid);

                if (customer == null)
                {
                    throw new Exception(
                        "Customer not found.");
                }

                var verificationFee = await _verificationFeeRepository.GetByTypeAsync(CustomerRegDocumentType.AADHAAR);
                var isAadhaarPaymentCompleted = await _aadhaarSessionRepository.HasCompletedPaymentAsync(customerUuid);

                var state = !string.IsNullOrWhiteSpace(customer.StateUUID)
                    ? await _stateService.GetByUuidAsync(customer.StateUUID)
                    : null;

                var city = !string.IsNullOrWhiteSpace(customer.CityUUID)
                    ? await _cityService.GetByUuidAsync(customer.CityUUID)
                    : null;

                var encryptedAadhaar = await _customerRegDocumentRepository.GetAadhaarDocumentAsync(customerUuid);

                string? aadhaar = null;
                string? maskedAadhaar = null;

                if (encryptedAadhaar != null && !string.IsNullOrWhiteSpace(encryptedAadhaar.RecordNo))
                {
                    var decryptedAadhaar = _encryptionService.Decrypt(encryptedAadhaar.RecordNo);
                    aadhaar ??= decryptedAadhaar;
                    maskedAadhaar = Utils.MaskAadhaar(decryptedAadhaar);
                }

                var response =
                    new RegistrationStateDto
                    {
                        CurrentStep = customer?.CurrentStep,
                        IsRegistered = customer?.IsRegistered ?? false,
                        AadhaarVerificationFee = Convert.ToDecimal(verificationFee?.Amount ?? 0),
                        IsAadhaarPaymentDone = isAadhaarPaymentCompleted,
                        // TODO: bind IsEmailVerified dynamically
                        IsEmailVerified = true,
                        AadhaarNumber = aadhaar ?? string.Empty,
                        IsVideoKycCompleted = await _customerVideoKycRepository.IsVerifiedAsync(customerUuid),
                        Customer =
                            new CustomerDtoForApi
                            {
                                FName = customer.FName,
                                LName = customer.LName,
                                Mobile = customer.Mobile,
                                EmailId = customer.Email,
                                IndustryUUID = customer.IndustryUUID,
                                StateUUID = customer.StateUUID,
                                CityUUID = customer.CityUUID,
                                ACType = customer.ACType,
                                StateName = state?.Title,
                                CityName = city?.Title,
                                MaskedAadhaar = maskedAadhaar,
                                Avtar = CustomerLookType.Cool
                            },
                        Business =
                            new RegistrationBusinessStateDto
                            {
                                BusinessTypeUUID =
                                    customer.BusinessTypeUUID,

                                IsBusinessVerified =
                                    customer.IsBusinessVerified,

                                IsDirectorSelected =
                                    customer.IsDirectorSelected
                            }
                    };

                switch (customer?.CurrentStep)
                {
                    case RegistrationSteps.SELECT_ACCOUNT_TYPE:
                        response.Customer =
                            new CustomerDtoForApi
                            {
                                ACType = customer.ACType,
                            };
                        break;

                    case RegistrationSteps.VERIFY_AADHAAR:
                        response.Customer =
                            new CustomerDtoForApi
                            {
                                ACType = customer.ACType,
                            };
                        response.States =
                            await _stateService
                                .GetDropdownAsync(
                                    x => x.Title);
                        response.States =
                            await _stateService
                                .GetDropdownAsync(
                                    x => x.Title);

                        response.Cities =
                            await _cityService
                                .GetCityDropDownWithFK();

                        response.BusinessCategories =
                            await _industryService
                                .GetDropdownAsync(
                                    x => x.Title);

                        break;

                    case RegistrationSteps.BASIC_INFO:

                        response.States =
                            await _stateService
                                .GetDropdownAsync(
                                    x => x.Title);

                        response.Cities =
                            await _cityService
                                .GetCityDropDownWithFK();

                        response.BusinessCategories =
                            await _industryService
                                .GetDropdownAsync(
                                    x => x.Title);

                        break;

                    case RegistrationSteps.VIDEO_KYC:

                        response.IsVideoKycCompleted = await _customerVideoKycRepository.IsVerifiedAsync(customerUuid);

                        break;

                    case RegistrationSteps.TERMS:

                        response.States =
                            await _stateService
                                .GetDropdownAsync(
                                    x => x.Title);

                        response.Cities =
                            await _cityService
                                .GetCityDropDownWithFK();

                        response.BusinessCategories =
                            await _industryService
                                .GetDropdownAsync(
                                    x => x.Title);

                        response.Policies = await _policyService.GetPolicyListForRegistrationAsync();

                        var consent =
                            await _customerConsentRepository
                                .GetLatestByCustomerUUIDAsync(
                                    customerUuid);

                        if (consent != null)
                        {
                            response.IsPolicySigned =
                                true;

                            response.SignId =
                                consent.SignId;

                            response.Signature =
                                consent.Signature;

                            response.SignedAt =
                                consent.SignedAt;
                        }

                        break;

                    case RegistrationSteps.DOS_DONTS:

                        var document =
                            await _masterDosDontsDocumentRepository
                                .GetActiveAsync();

                        if (document == null)
                        {
                            throw new Exception(
                                "Active Do's & Don'ts document not found.");
                        }

                        var dos =
                            await _dosDontsService
                                .GetDosDontsListAsync(
                                    document.UUID,
                                    true);

                        var donts =
                            await _dosDontsService
                                .GetDosDontsListAsync(
                                    document.UUID,
                                    false);

                        var customerConsent =
                            await _customerConsentDosDontsRepository
                                .GetByCustomerUUIDAsync(
                                    customerUuid);

                        var isAccepted = await _customerConsentDosDontsRepository
                                .HasAcceptedDocumentAsync(
                                    customerUuid,
                                    document.UUID);

                        response.DosDonts =
                            new DosDontsDocumentDto
                            {
                                DocumentUUID =
                                    document.UUID,

                                VersionNo =
                                    document.VersionNo,

                                IsAccepted =
                                    isAccepted,

                                Dos =
                                    dos
                                        .Select(x =>
                                            new DosDontsItemDto
                                            {
                                                Message =
                                                    x.Message,

                                                SequenceNo =
                                                    x.SequenceNo
                                            })
                                        .ToList(),

                                Donts =
                                    donts
                                        .Select(x =>
                                            new DosDontsItemDto
                                            {
                                                Message =
                                                    x.Message,

                                                SequenceNo =
                                                    x.SequenceNo
                                            })
                                        .ToList()
                            };

                        break;

                }

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
