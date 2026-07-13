
using AuthenticateIndia.Shared.Constants;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Upgrow.Application.Commands;
using Upgrow.Application.DTO;
using Upgrow.Application.DTO.Cart;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.DTO.Transaction;
using Upgrow.Application.Helper;
using Upgrow.Application.Interfaces;
using Upgrow.Application.Interfaces.Pricing;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.CustomerPanel;
using Upgrow.Application.Services.Master;
using Upgrow.Domain;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Registration;
using Upgrow.Domain.IRepositories;
using Upgrow.Domain.IRepositories.Master;
using Upgrow.Domain.IRepositories.Registration;

namespace Upgrow.Application.Services.CustomerPanel
{
    public class CartService : ICartService
    {
        private readonly ICustomerCreditRepository _customerCreditRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IMasterCustomerRepository _masterCustomerRepository;
        private readonly ICustomerRegDocumentRepository _customerRegDocumentRepository;
        private readonly IMasterApiRepository _masterApiRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMasterPricingRepository _pricingRepository;
        private readonly IVerificationFeeRepository _verificationFeeRepository;
        private readonly ICommonService _commonService;

        public CartService(
            ICustomerCreditRepository customerCreditRepository,
            IEncryptionService encryptionService,
            IMasterCustomerRepository masterCustomerRepository,
            ICustomerRegDocumentRepository customerRegDocumentRepository,
            IMasterApiRepository masterApiRepository,
            ICartRepository cartRepository,
            IMasterPricingRepository pricingRepository,
            IVerificationFeeRepository verificationFeeRepository,
            ICommonService commonService)
        {
            _customerCreditRepository =
                customerCreditRepository;

            _encryptionService =
                encryptionService;

            _masterCustomerRepository =
                masterCustomerRepository;

            _customerRegDocumentRepository =
                customerRegDocumentRepository;

            _masterApiRepository =
                masterApiRepository;

            _cartRepository =
                cartRepository;

            _pricingRepository =
                pricingRepository;

            _verificationFeeRepository =
                verificationFeeRepository;

            _commonService =
                commonService;
        }

        private async Task PopulateSelfConsentAsync(
    Master_Cart cart)
        {
            var customer =
                await _masterCustomerRepository
                    .GetByUuidAsync(
                        cart.VerifierUUID);

            if (customer == null)
            {
                throw new Exception(
                    "Customer not found.");
            }

            var aadhaar =
                await _customerRegDocumentRepository
                    .GetAadhaarDocumentAsync(
                        cart.VerifierUUID);

            if (aadhaar == null)
            {
                throw new Exception(
                    "Aadhaar document not found.");
            }

            cart.ConsentDocNo =
                aadhaar.RecordNo;

            cart.ConsentMobileNo =
                customer.Mobile;
        }

        private async Task RecalculateCartAsync(
            Master_Cart cart)
        {
            var details =
                await _cartRepository
                    .GetCartDetailsAsync(
                        cart.UUID);

            var hasConsentApi =
                details.Any(x =>
                    x.IsConsentBased);

            Master_VerificationFee? consentFee = null;

            if (hasConsentApi)
            {
                consentFee =
                    await _verificationFeeRepository
                        .GetByTypeAsync(
                            VerificationFeeConstants.CONSENT_FEE);
            }

            cart.TotalApis =
                details.Count;

            cart.BaseCreditsTotal =
                details.Sum(x =>
                    x.ApiCharge);

            cart.ConsentCreditsTotal =
                hasConsentApi
                    ? consentFee?.Amount ?? 0
                    : 0;

            if (cart.AuthFor == ConsentFor.SELF &&
                (string.IsNullOrWhiteSpace(cart.ConsentDocNo) ||
                 string.IsNullOrWhiteSpace(cart.ConsentMobileNo)))
            {
                await PopulateSelfConsentAsync(cart);
            }

            cart.PayableBaseAmount =
                cart.BaseCreditsTotal +
                cart.ConsentCreditsTotal;
        }


        public async Task<string> AddToCartAsync(
            AddToCartRequest request,
            string verifierUuid)
        {
            var platformOwnerUuid =
                await _commonService
                    .GetPlatformOwnerUUID();

            if (string.IsNullOrWhiteSpace(
                platformOwnerUuid))
            {
                throw new Exception(
                    "Platform owner not found.");
            }

            var api = await _masterApiRepository.GetByUuidAsync(
            request.ApiUUID);

            if (api == null)
            {
                throw new Exception(
                    "API not found.");
            }

            if (!api.IsActive)
            {
                throw new Exception(
                    "API is inactive.");
            }

            var pricing =
                await _pricingRepository
                    .GetCustomerPricingQuery(
                        DateOnly.FromDateTime(
                            DateTime.UtcNow),
                        platformOwnerUuid)
                    .FirstOrDefaultAsync(x =>
                        x.APIUUID ==
                        request.ApiUUID);

            if (pricing == null)
            {
                throw new Exception(
                    "Pricing not configured.");
            }

            var cart =
                await _cartRepository
                    .GetPendingCartAsync(verifierUuid);

            if (cart == null)
            {
                if (string.IsNullOrWhiteSpace(request.AuthFor))
                {
                    throw new Exception(
                        "AuthFor is required.");
                }

                cart = new Master_Cart
                {
                    UUID =
                        Utils.GetUUID(),

                    CartNo =
                        $"CRT-{DateTime.UtcNow:yyyyMMddHHmmssfff}",

                    AuthFor =
                        request.AuthFor,

                    BaseCreditsTotal = 0,

                    ConsentCreditsTotal = 0,

                    PayableBaseAmount = 0,

                    ConsentDocNo = null,

                    ConsentMobileNo = null,

                    VerifierUUID =
                        verifierUuid,

                    TotalApis = 0,

                    Status = CartStatusConstants.PENDING,

                    SellerTenantUUID =
                        platformOwnerUuid,

                    CreatedAt =
                        DateTimeOffset.UtcNow,

                    IsActive = true
                };


                if (request.AuthFor == ConsentFor.SELF)
                {
                    await PopulateSelfConsentAsync(
                        cart);
                }

                await _cartRepository
                    .AddCartAsync(cart);
            }

            var exists =
                await _cartRepository
                    .ExistsInCartAsync(
                        cart.UUID,
                        request.ApiUUID);

            if (exists)
            {
                throw new Exception(
                    "API already exists in cart.");
            }

            var detail =
                new CartDetail
                {
                    UUID =
                        Utils.GetUUID(),

                    ApiUUID =
                        request.ApiUUID,

                    CartUUID =
                        cart.UUID,

                    PricingUUID =
                        pricing.PricingUUID,

                    ApiCharge =
                        pricing.CurrentPrice ?? 0,

                    ReqPayload =
                        request.ReqPayload,

                    IsConsentBased =
                        api.IsConsentBased ?? false,

                    IsMultipleEndPoint =
                        api.IsMultipleEndPoint,

                    CreatedAt =
                        DateTimeOffset.UtcNow,

                    IsActive = true
                };

            await _cartRepository.AddCartDetailAsync(detail);

            await _cartRepository
                .SaveChangesAsync();

            await RecalculateCartAsync(cart);

            await _cartRepository.UpdateCartAsync(cart);

            await _cartRepository
                .SaveChangesAsync();

            return cart.UUID;
        }

        public async Task<CartSummaryDto?> GetCartSummaryAsync(
    string verifierUuid)
        {
            var cart =
                await _cartRepository
                    .GetPendingCartAsync(
                        verifierUuid);

            if (cart == null)
            {
                return new CartSummaryDto();
            }

            var items =
                await _cartRepository
                    .GetCartSummaryItemsAsync(
                        cart.UUID);

            var isConsentRequired =
                items.Any(x =>
                    x.IsConsentBased);

            var isConsentProvided =
                isConsentRequired
                &&
                !string.IsNullOrWhiteSpace(
                    cart.ConsentDocNo)
                &&
                !string.IsNullOrWhiteSpace(
                    cart.ConsentMobileNo);

            var baseCreditsTotal =
                items.Sum(x =>
                    x.ApiCharge);

            var consentCreditsTotal =
                isConsentRequired
                    ? cart.ConsentCreditsTotal
                    : 0;

            string? decryptedAadhaar = null;
            if (!string.IsNullOrEmpty(cart.ConsentDocNo))
            {
                decryptedAadhaar = _encryptionService.Decrypt(cart.ConsentDocNo);
            }

            return new CartSummaryDto
            {
                CartUUID =
                    cart.UUID,

                CartNo =
                    cart.CartNo,

                AuthFor =
                    cart.AuthFor,

                TotalApis =
                    items.Count,

                BaseCreditsTotal =
                    baseCreditsTotal,

                ConsentCreditsTotal =
                    consentCreditsTotal,

                PayableBaseAmount =
                    baseCreditsTotal +
                    consentCreditsTotal,

                IsConsentRequired =
                    isConsentRequired,

                IsConsentProvided =
                    isConsentProvided,

                ConsentDocNo =
                    decryptedAadhaar,

                ConsentMobileNo =
                    cart.ConsentMobileNo,

                Items =
                    items
            };
        }

        public async Task RemoveApiAsync(
    string verifierUuid,
    string apiUuid)
        {
            var cart =
                await _cartRepository
                    .GetPendingCartAsync(
                        verifierUuid);

            if (cart == null)
            {
                throw new Exception(
                    "Cart not found.");
            }

            var detail =
                await _cartRepository
                    .GetCartDetailAsync(
                        cart.UUID,
                        apiUuid);

            if (detail == null)
            {
                throw new Exception(
                    "API not found in cart.");
            }

            await _cartRepository
                .DeleteCartDetailAsync(
                    detail);

            await _cartRepository
                .SaveChangesAsync();

            await RecalculateCartAsync(cart);

            var remainingApis =
                await _cartRepository
                    .GetCartDetailsAsync(
                        cart.UUID);

            if (!remainingApis.Any())
            {
                cart.Status =
                    CartStatusConstants.DEACTIVATED;

                cart.IsActive = false;

                cart.ConsentDocNo = null;

                cart.ConsentMobileNo = null;
            }
            else
            {
                var hasConsentApi =
                    remainingApis.Any(x =>
                        x.IsConsentBased);

                if (!hasConsentApi)
                {
                    cart.ConsentDocNo = null;

                    cart.ConsentMobileNo = null;
                }
            }

            await _cartRepository
                .UpdateCartAsync(cart);

            await _cartRepository
                .SaveChangesAsync();
        }

        public async Task<SaveConsentDetailsResponseDto> SaveConsentDetailsAsync(
            string verifierUuid,
            SaveConsentDetailsRequest request)
        {
            var cart =
                await _cartRepository
                    .GetPendingCartAsync(
                        verifierUuid);

            if (cart == null)
            {
                throw new Exception(
                    "Cart not found.");
            }

            if (cart.AuthFor == ConsentFor.SELF)
            {
                return new SaveConsentDetailsResponseDto
                {
                    AuthFor = cart.AuthFor,
                    IsConsentProvided = true
                };
            }

            var details =
                await _cartRepository
                    .GetCartDetailsAsync(
                        cart.UUID);

            var consentRequired =
                details.Any(x =>
                    x.IsConsentBased);

            if (!consentRequired)
            {
                throw new Exception(
                    "Consent is not required.");
            }


            if (string.IsNullOrWhiteSpace(
                        request.ConsentDocNo))
            {
                throw new Exception(
                    "Consent document number is required.");
            }

            if (string.IsNullOrWhiteSpace(
                    request.ConsentMobileNo))
            {
                throw new Exception(
                    "Consent mobile number is required.");
            }

            cart.ConsentDocNo =
                _encryptionService.Encrypt(
                    request.ConsentDocNo);

            cart.ConsentMobileNo =
                request.ConsentMobileNo;

            await _cartRepository.UpdateCartAsync(cart);

            await _cartRepository.SaveChangesAsync();

            return new SaveConsentDetailsResponseDto
            {
                AuthFor = cart.AuthFor,

                IsConsentProvided = true
            };
        }

        

        public async Task<GetPaymentOptionsResponseDto> GetPaymentOptionsAsync(
    string verifierUuid)
        {
            var cart =
                await _cartRepository
                    .GetPendingCartAsync(
                        verifierUuid);

            if (cart == null)
            {
                throw new Exception(
                    "Cart not found.");
            }

            var cartDetails =
                await _cartRepository
                    .GetCartDetailsAsync(
                        cart.UUID);

            if (!cartDetails.Any())
            {
                throw new Exception(
                    "Cart is empty.");
            }

            var creditMaster =
                await _customerCreditRepository
                    .GetByCustomerUuidAsync(
                        verifierUuid);

            var availableCredits =
                creditMaster?.CurrentBalance ?? 0;

            var consentRequired =
                cartDetails.Any(x =>
                    x.IsConsentBased);

            string? aadhaarNumber = null;
            string? maskedAadhaar = null;
            string? consentMobile = null;

            if (consentRequired)
            {
                if (!string.IsNullOrWhiteSpace(
                        cart.ConsentDocNo))
                {
                    var decryptedAadhaar =
                        _encryptionService
                            .Decrypt(
                                cart.ConsentDocNo);

                    maskedAadhaar =
                        Utils.MaskAadhaar(
                            decryptedAadhaar);

                    aadhaarNumber = decryptedAadhaar;
                }

                consentMobile =
                    cart.ConsentMobileNo;
            }

            decimal serviceCredits =
                cart.BaseCreditsTotal;

            decimal consentCredits =
                cart.ConsentCreditsTotal;

            decimal totalCredits =
                serviceCredits +
                consentCredits;

            decimal gstPercentage =
                GstConstants.GST_PERCENTAGE;

            decimal gstAmount =
                Math.Round(
                    totalCredits *
                    gstPercentage /
                    100,
                    2);

            decimal onlineTotal =
                totalCredits +
                gstAmount;

            return new GetPaymentOptionsResponseDto
            {
                AadhaarNumber = aadhaarNumber,

                MaskedAadhaarNumber =
                    maskedAadhaar,

                ConsentMobileNumber =
                    consentMobile,

                PaymentMethods =
                [
                    new PaymentMethodDto
            {
                Code =
                    PaymentModeConstants.ONLINE,

                Title =
                    "Online Payment",

                Enabled = true,

                AvailableCredits =
                    availableCredits,

                CreditSummary =
                    new CreditSummaryDto
                    {
                        ServiceCredits =
                            serviceCredits,

                        ConsentCredits =
                            consentCredits,

                        TotalCredits =
                            totalCredits
                    },

                PaymentSummary =
                    new PaymentSummaryDto
                    {
                        Subtotal =
                            totalCredits,

                        GstPercentage =
                            gstPercentage,

                        GstAmount =
                            gstAmount,

                        TotalPayable =
                            onlineTotal
                    }
            },

            new PaymentMethodDto
            {
                Code =
                    PaymentModeConstants.CREDIT,

                Title =
                    "Use Credit",

                Enabled = true,

                AvailableCredits =
                    availableCredits,

                CreditSummary =
                    new CreditSummaryDto
                    {
                        ServiceCredits =
                            serviceCredits,

                        ConsentCredits =
                            consentCredits,

                        TotalCredits =
                            totalCredits
                    },

                PaymentSummary =
                    new PaymentSummaryDto
                    {
                        Subtotal =
                            totalCredits,

                        GstPercentage = 0,

                        GstAmount = 0,

                        TotalPayable =
                            totalCredits
                    }
            }
                ]
            };
        }
    }
}