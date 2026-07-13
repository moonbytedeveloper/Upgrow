using Upgrow.Shared.Constants;
using Upgrow.Shared.Constants.TransactionDocument;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Common;
using Upgrow.Application.DTO.Transaction;
using Upgrow.Application.Interfaces;
using Upgrow.Application.Interfaces.Notification;
using Upgrow.Application.Interfaces.Registration;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Payment;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;
using Upgrow.Domain.IRepositories.Master;
using Upgrow.Domain.IRepositories.Registration;
using static Upgrow.Application.Constants;

namespace Upgrow.Application.Services.CustomerPanel
{
    public class TransactionService
        : ITransactionService
    {
        private readonly IAppSettingService _appSettingService;

        private readonly IAadhaarVerificationService _aadhaarVerificationService;

        private readonly IMasterPolicyService
            _masterPolicyService;

        private readonly IEncryptionService
            _encryptionService;

        private readonly IMasterCustomerRepository
    _masterCustomerRepository;

        private readonly ICustomerRegDocumentRepository
            _customerRegDocumentRepository;

        private readonly IRazorpayService
            _razorpayService;

        private readonly ICartRepository
            _cartRepository;

        private readonly ITransactionRepository
            _transactionRepository;

        private readonly ICustomerCreditRepository
            _customerCreditRepository;

        private readonly IConsentNotificationService
            _consentNotificationService;

        private readonly IConfiguration
            _configuration;

        private readonly IMasterApiRepository
    _masterApiRepository;

        public TransactionService(
            IAppSettingService appSettingService,
            IAadhaarVerificationService aadhaarVerificationService,
            IMasterPolicyService masterPolicyService,
            IEncryptionService encryptionService,
            IMasterCustomerRepository masterCustomerRepository,
            ICustomerRegDocumentRepository customerRegDocumentRepository,
            IRazorpayService razorpayService,
            IMasterApiRepository masterApiRepository,
            ICartRepository cartRepository,
            ITransactionRepository transactionRepository,
            ICustomerCreditRepository customerCreditRepository,
            IConsentNotificationService consentNotificationService,
            IConfiguration configuration)
        {
            _appSettingService = appSettingService;

            _aadhaarVerificationService = aadhaarVerificationService;
            _masterPolicyService = masterPolicyService;
            _encryptionService =
                encryptionService;

            _masterCustomerRepository =
    masterCustomerRepository;

            _customerRegDocumentRepository =
                customerRegDocumentRepository;
            _razorpayService =
                razorpayService;

            _masterApiRepository =
                masterApiRepository;

            _cartRepository =
                cartRepository;

            _transactionRepository =
                transactionRepository;

            _customerCreditRepository =
                customerCreditRepository;

            _consentNotificationService =
                consentNotificationService;

            _configuration =
                configuration;
        }

        

        

        private async Task DeductCreditsAsync(
            string customerUuid,
            string transactionUuid,
            decimal amount)
        {
            var creditMaster =
                await _customerCreditRepository
                    .GetByCustomerUuidAsync(
                        customerUuid);

            if (creditMaster == null)
            {
                throw new Exception(
                    "Credit account not found.");
            }

            if (creditMaster.CurrentBalance < amount)
            {
                throw new Exception(
                    "Insufficient credits.");
            }

            var balanceBefore =
                creditMaster.CurrentBalance;

            creditMaster.CurrentBalance -= amount;

            creditMaster.TotalCreditsConsumed += amount;

            creditMaster.LastTransactionAt =
                DateTimeOffset.UtcNow;

            await _customerCreditRepository
                .UpdateCreditMasterAsync(
                    creditMaster);

            await _customerCreditRepository
                .AddDebitLedgerAsync(
                    new CustomerDebitLedger
                    {
                        UUID =
                            Utils.GetUUID(),

                        CustomerUUID =
                            customerUuid,

                        TransactionUUID =
                            transactionUuid,

                        DebitType =
                            DebitTypeConstants.Verification,

                        CreditsDebited =
                            amount,

                        BalanceBefore =
                            balanceBefore,

                        BalanceAfter =
                            creditMaster.CurrentBalance,

                        Remarks =
                            $"Credits used for transaction {transactionUuid}",

                        CreatedAt =
                            DateTimeOffset.UtcNow,

                        IsActive = true
                    });
        }

        public async Task<CreatePaymentOrderResponseDto> CreatePaymentOrderAsync(
            string transactionUuid)
        {
            var transaction =
                await _transactionRepository
                    .GetByUuidAsync(
                        transactionUuid);

            if (transaction == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            if (transaction.PaymentMode !=
                PaymentModeConstants.ONLINE)
            {
                throw new Exception(
                    "Payment order can only be created for ONLINE mode.");
            }

            if (transaction.PaymentStatus ==
                PaymentStatusConstants.Paid)
            {
                throw new Exception(
                    "Transaction already paid.");
            }

            var existingPayments = await _transactionRepository.GetPaymentHistoryAsync(transaction.UUID);

            foreach (var payment in existingPayments)
            {
                payment.IsCurrent = false;

                await _transactionRepository.UpdatePaymentRecordAsync(payment);
            }

            var order =
                await _razorpayService
                    .CreateOrderAsync(
                        transaction.TotalPayableAmount,
                        transaction.TransactionNo);

            var paymentRecord =
                new TransactionRzpPGRecord
                {
                    UUID =
                        Utils.GetUUID(),

                    TransactionUUID =
                        transaction.UUID,

                    OrderId =
                        order.OrderId,

                    OrderAmount =
                        order.Amount,

                    OrderStatus =
                        RazorpayOrderStatusConstants.Created,

                    PaymentStatus =
                        RazorpayPaymentStatusConstants.Pending,

                    OrderCreatedAt =
                        DateTimeOffset.UtcNow,

                    IsCurrent =
                        true,

                    IsActive =
                        true
                };

            await _transactionRepository
                .AddPaymentRecordAsync(
                    paymentRecord);

            await _transactionRepository
                .SaveChangesAsync();

            return new CreatePaymentOrderResponseDto
            {
                TransactionUUID =
                    transaction.UUID,

                OrderId =
                    order.OrderId,

                Amount =
                    order.Amount,

                KeyId =
                    order.KeyId
            };
        }

        public async Task<VerifyPaymentResponseDto> VerifyPaymentAsync(
            VerifyPaymentRequest request)
        {
            var transaction =
                await _transactionRepository
                    .GetByUuidAsync(
                        request.TransactionUUID);

            if (transaction == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            var payment =
                await _transactionRepository
                    .GetCurrentPaymentAsync(
                        request.TransactionUUID);

            if (payment == null)
            {
                throw new Exception(
                    "Payment record not found.");
            }

            var verified =
                _razorpayService
                    .VerifySignature(
                        request.RazorpayOrderId,
                        request.RazorpayPaymentId,
                        request.RazorpaySignature);

            if (!verified)
            {
                throw new Exception(
                    "Invalid payment signature.");
            }

            payment.PaymentId =
                request.RazorpayPaymentId;

            payment.PaymentStatus =
                RazorpayPaymentStatusConstants.Success;

            payment.OrderStatus =
                RazorpayOrderStatusConstants.Paid;

            payment.PaymentCreatedAt =
                DateTimeOffset.UtcNow;

            transaction.PaymentStatus =
                PaymentStatusConstants.Paid;


            var cart = await _cartRepository.GetPendingCartAsync(transaction.VerifierUUID);

            if (cart != null)
            {
                cart.IsActive = false;
                cart.Status = CartStatusConstants.CHECKOUT;
                await _cartRepository.UpdateCartAsync(cart);
            }

            await _transactionRepository
                .UpdatePaymentRecordAsync(
                    payment);

            await _transactionRepository
                .UpdateTransactionAsync(
                    transaction);

            await _transactionRepository
                .SaveChangesAsync();

            return new VerifyPaymentResponseDto
            {
                TransactionUUID = transaction.UUID,

                PaymentStatus = transaction.PaymentStatus,

                IsPaymentSuccess = true
            };
        }

        public async Task<InitiateTransactionResponseDto> InitiateTransactionAsync(
            string verifierUuid,
            InitiateTransactionRequest request)
        {
            using var dbTransaction =
                await _transactionRepository
                    .BeginTransactionAsync();

            try
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

                var paidTransaction =
    await _transactionRepository
        .GetPaidTransactionByCartUuidAsync(
            cart.UUID);

                if (paidTransaction != null)
                {
                    throw new Exception(
                        "Payment has already been completed for this cart.");
                }

                var activeTransactions =
                    await _transactionRepository
                        .GetActiveTransactionsByCartUuidAsync(
                            cart.UUID);

                foreach (var existingTransaction in activeTransactions)
                {
                    existingTransaction.IsActive = false;

                    existingTransaction.PaymentStatus =
                        PaymentStatusConstants.Cancelled;

                    await _transactionRepository
                        .UpdateTransactionAsync(
                            existingTransaction);

                    var consent =
                        await _transactionRepository
                            .GetConsentByTransactionUuidAsync(
                                existingTransaction.UUID);

                    if (consent != null)
                    {
                        consent.IsActive = false;

                        consent.ConsentStatus =
                            ConsentStatusConstants.Cancelled;

                        await _transactionRepository
                            .UpdateTransactionConsentAsync(
                                consent);
                    }

                    var payments =
                        await _transactionRepository
                            .GetPaymentHistoryAsync(
                                existingTransaction.UUID);

                    foreach (var payment in payments)
                    {
                        payment.IsCurrent = false;

                        payment.OrderStatus =
                            RazorpayOrderStatusConstants.Cancelled;

                        payment.PaymentStatus =
                            RazorpayPaymentStatusConstants.Cancelled;

                        await _transactionRepository
                            .UpdatePaymentRecordAsync(
                                payment);
                    }
                }

                var cartDetails = await _cartRepository
                                    .GetCartTransactionDetailsAsync(
                                        cart.UUID);

                if (!cartDetails.Any())
                {
                    throw new Exception(
                        "Cart is empty.");
                }

                var consentRequired =
                    cartDetails.Any(x =>
                        x.IsConsentBased);

                if (consentRequired)
                {
                    if (string.IsNullOrWhiteSpace(
                            cart.ConsentDocNo))
                    {
                        throw new Exception(
                            "Consent details not provided.");
                    }

                    if (string.IsNullOrWhiteSpace(
                            cart.ConsentMobileNo))
                    {
                        throw new Exception(
                            "Consent details not provided.");
                    }
                }

                if (request.PaymentMode !=
                        PaymentModeConstants.ONLINE
                    &&
                    request.PaymentMode !=
                        PaymentModeConstants.CREDIT)
                {
                    throw new Exception(
                        "Invalid payment mode.");
                }

                decimal gst = 0;
                decimal cgst = 0;
                decimal sgst = 0;
                decimal igst = 0;

                if (request.PaymentMode ==
                    PaymentModeConstants.ONLINE)
                {
                    gst =
                        Math.Round(
                            cart.PayableBaseAmount *
                            (GstConstants.GST_PERCENTAGE / 100),
                            2);

                    cgst =
                        Math.Round(
                            gst / 2,
                            2);

                    sgst =
                        Math.Round(
                            gst / 2,
                            2);
                }

                if (request.PaymentMode ==
                    PaymentModeConstants.CREDIT)
                {
                    var hasCredits =
                        await _customerCreditRepository
                            .HasSufficientCreditsAsync(
                                verifierUuid,
                                cart.PayableBaseAmount);

                    if (!hasCredits)
                    {
                        throw new Exception(
                            "Insufficient credits.");
                    }
                }

                var transaction =
                    new Transaction
                    {
                        UUID =
                            Utils.GetUUID(),

                        TransactionNo =
                            $"TXN-{DateTime.UtcNow:yyyyMMddHHmmssfff}",

                        CartUUID =
                            cart.UUID,

                        AuthFor =
                            cart.AuthFor,

                        BaseCreditsTotal =
                            cart.BaseCreditsTotal,

                        ConsentCreditsTotal =
                            cart.ConsentCreditsTotal,

                        PayableBaseAmount =
                            cart.PayableBaseAmount,

                        GST =
                            gst,

                        CGST =
                            cgst,

                        SGST =
                            sgst,

                        IGST =
                            igst,

                        TotalPayableAmount =
                            cart.PayableBaseAmount + gst,

                        PaymentMode =
                            request.PaymentMode,

                        VerifierUUID =
                            verifierUuid,

                        PaymentStatus =
                            request.PaymentMode ==
                            PaymentModeConstants.CREDIT
                                ? PaymentStatusConstants.Paid
                                : PaymentStatusConstants.Pending,

                        TransactionStatus =
                            consentRequired
                                ? TransactionStatusConstants.ConsentPending
                                : TransactionStatusConstants.ReadyForExecution,

                        ProcessingStatus = TransactionProcessingStatusConstants.Pending,

                        CreatedAt = DateTimeOffset.UtcNow,

                        IsActive =
                            true
                    };

                await _transactionRepository
                    .AddTransactionAsync(
                        transaction);

                foreach (var item in cartDetails)
                {
                    await _transactionRepository
                        .AddTransactionDetailAsync(
                            new TransactionDetail
                            {
                                UUID =
                                    Utils.GetUUID(),

                                TransactionUUID =
                                    transaction.UUID,

                                ApiUUID =
                                    item.ApiUUID,

                                ReqPl =
                                    item.ReqPayload,

                                VerificationCode = 
                                    item.VerificationCode,

                                CurrentVerificationCode =
                                    item.VerificationCode,

                                PricingUUID =
                                    item.PricingUUID,

                                ApiCharge =
                                    item.ApiCharge,

                                RecordDatetime =
                                    DateTimeOffset.UtcNow,

                                IsMultistageRoute =
                                    item.IsMultipleEndPoint,

                                IsConsentRequired =
                                    item.IsConsentBased,

                                ProcessingStatus =
                                    TransactionDetailProcessingStatusConstants.Pending,

                                IsActive =
                                    true
                            });
                }

                if (consentRequired)
                {
                    await _transactionRepository
                        .AddTransactionConsentAsync(
                            new TransactionConsent
                            {
                                UUID =
                                    Utils.GetUUID(),

                                TransactionUUID =
                                    transaction.UUID,

                                ConsentDocNo =
                                    cart.ConsentDocNo!,

                                ConsentMobileNo =
                                    cart.ConsentMobileNo!,

                                ConsentStatus =
                                    ConsentStatusConstants.Pending,

                                CreatedAt =
                                    DateTimeOffset.UtcNow,

                                IsActive = true
                            });
                }

                if (request.PaymentMode ==
                    PaymentModeConstants.CREDIT)
                {
                    await DeductCreditsAsync(
                        verifierUuid,
                        transaction.UUID,
                        cart.PayableBaseAmount);

                    cart.Status = CartStatusConstants.CHECKOUT;
                    cart.IsActive = false;

                    await _cartRepository.UpdateCartAsync(cart);
                }


                await _transactionRepository
                    .SaveChangesAsync();

                await dbTransaction
                    .CommitAsync();

                return new InitiateTransactionResponseDto
                {
                    TransactionUUID =
                        transaction.UUID,

                    TransactionNo =
                        transaction.TransactionNo,

                    PaymentMode =
                        transaction.PaymentMode,

                    PaymentStatus =
                        transaction.PaymentStatus,

                    BaseCreditsTotal =
                        transaction.BaseCreditsTotal,

                    ConsentCreditsTotal =
                        transaction.ConsentCreditsTotal,

                    PayableBaseAmount =
                        transaction.PayableBaseAmount,

                    GST =
                        transaction.GST,

                    TotalPayableAmount =
                        transaction.TotalPayableAmount,

                    ConsentRequired =
                        consentRequired,

                    PaymentRequired =
                        transaction.TotalPayableAmount > 0 &&
                        request.PaymentMode ==
                        PaymentModeConstants.ONLINE
                };
            }
            catch
            {
                await dbTransaction
                    .RollbackAsync();

                throw;
            }
        }

        public async Task<GenerateConsentLinkResponseDto> GenerateConsentLinkAsync(
            string transactionUuid)
        {
            var transaction = await _transactionRepository.GetByUuidAsync(transactionUuid);

            if (transaction == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            if (transaction.PaymentStatus !=
                PaymentStatusConstants.Paid)
            {
                throw new Exception(
                    "Payment is pending.");
            }

            var consent =
                await _transactionRepository
                    .GetConsentByTransactionUuidAsync(
                        transactionUuid);

            if (consent == null)
            {
                throw new Exception(
                    "Consent record not found.");
            }

            if (!consent.IsActive)
            {
                throw new Exception(
                    "Consent record is inactive.");
            }

            if (consent.ConsentStatus ==
                ConsentStatusConstants.Approved)
            {
                throw new Exception(
                    "Consent already approved.");
            }

            if (consent.ConsentStatus ==
                ConsentStatusConstants.Rejected)
            {
                throw new Exception(
                    "Consent already rejected.");
            }

            string token;

            if (!string.IsNullOrWhiteSpace(consent.ConsentToken)
                &&
                consent.ConsentExpiresAt > DateTimeOffset.UtcNow)
            {
                token = consent.ConsentToken;
            }
            else
            {
                token = Utils.GenerateToken();
            }

            var consentExpiryInSeconds = await _appSettingService.GetIntValueAsync(AppSettingKeys.CONSENT_LINK_EXPIRY_SECONDS, 540);

            if (consentExpiryInSeconds <= 0)
            {
                consentExpiryInSeconds = 540; // Default to 9 minutes
            }

            consent.ConsentToken =
                token;

            consent.ConsentExpiresAt =
                DateTimeOffset.UtcNow
                    .AddSeconds(consentExpiryInSeconds);

            consent.IsExpired =
                false;

            await _transactionRepository
                .UpdateTransactionConsentAsync(
                    consent);

            await _transactionRepository
                .SaveChangesAsync();

            var baseUrl =
                _configuration[
                    "ConsentSettings:BaseUrl"];

            if (string.IsNullOrWhiteSpace(
                    baseUrl))
            {
                throw new Exception(
                    "ConsentSettings:BaseUrl is not configured.");
            }

            var consentUrl = string.Format(baseUrl, token);

            return new GenerateConsentLinkResponseDto
            {
                TransactionUUID =
                    transactionUuid,

                ConsentToken =
                    token,

                ConsentUrl =
                    consentUrl,

                ConsentExpiresAt =
                    consent.ConsentExpiresAt.Value,

                ConsentExpiresInSeconds = await _appSettingService.GetIntValueAsync(AppSettingKeys.CONSENT_LINK_EXPIRY_SECONDS, 540)
            };
        }

        public async Task<SendConsentLinkResponseDto> SendConsentLinkAsync(
            string transactionUuid)
        {
            var transaction = await _transactionRepository.GetByUuidAsync(transactionUuid);

            if (transaction == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            if (transaction.PaymentStatus !=
                PaymentStatusConstants.Paid)
            {
                throw new Exception(
                    "Payment is pending.");
            }

            var consent =
                await _transactionRepository
                    .GetConsentByTransactionUuidAsync(
                        transactionUuid);

            if (consent == null)
            {
                throw new Exception(
                    "Consent record not found.");
            }

            if (!consent.IsActive)
            {
                throw new Exception(
                    "Consent record is inactive.");
            }

            if (consent.ConsentStatus ==
                ConsentStatusConstants.Approved)
            {
                throw new Exception(
                    "Consent already approved.");
            }

            if (consent.ConsentStatus ==
                ConsentStatusConstants.Rejected)
            {
                throw new Exception(
                    "Consent already rejected.");
            }

            GenerateConsentLinkResponseDto
                consentLink;

            if (string.IsNullOrWhiteSpace(
                    consent.ConsentToken)
                ||
                consent.ConsentExpiresAt ==
                    null
                ||
                consent.ConsentExpiresAt <=
                    DateTimeOffset.UtcNow)
            {
                consentLink =
                    await GenerateConsentLinkAsync(
                        transactionUuid);
            }
            else
            {
                var baseUrl =
                    _configuration[
                        "ConsentSettings:BaseUrl"];

                consentLink =
                    new GenerateConsentLinkResponseDto
                    {
                        TransactionUUID =
                            transactionUuid,

                        ConsentToken =
                            consent.ConsentToken,

                        ConsentUrl =
                            $"{baseUrl}/{consent.ConsentToken}",

                        ConsentExpiresAt =
                            consent.ConsentExpiresAt
                                .Value
                    };
            }

            await _consentNotificationService
                .SendConsentLinkAsync(
                    transaction.VerifierUUID,
                    consent.ConsentToken);

            consent.IsConsentSentOnMobile =
                true;

            consent.ConsentLinkSentAt =
                DateTimeOffset.UtcNow;

            await _transactionRepository
                .UpdateTransactionConsentAsync(
                    consent);

            await _transactionRepository
                .SaveChangesAsync();

            return new
                SendConsentLinkResponseDto
            {
                TransactionUUID =
                    transactionUuid,

                ConsentMobileNo =
                    consent.ConsentMobileNo,

                IsConsentSent = true,

                ConsentLinkSentAt =
                    consent.ConsentLinkSentAt
                        .Value
            };
        }

        public async Task<GetConsentStatusResponseDto> GetConsentStatusAsync(
            string verifierUuid,
            string transactionUuid)
        {
            var transaction =
                await _transactionRepository
                    .GetByUuidAsync(
                        transactionUuid);

            if (transaction == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            if (transaction.VerifierUUID !=
                verifierUuid)
            {
                throw new Exception(
                    "Unauthorized transaction.");
            }

            var response =
                new GetConsentStatusResponseDto
                {
                    TransactionUUID =
                        transaction.UUID
                };

            /*//
            // No consent required
            //
            if (transaction.AuthFor ==
                ConsentFor.SELF)
            {
                response.ConsentRequired = false;

                response.ConsentStatus =
                    ConsentStatusConstants.NotRequired;

                response.IsCompleted = true;

                response.Message =
                    "Consent is not required.";

                return response;
            }*/

            var consent =
                await _transactionRepository
                    .GetConsentByTransactionUuidAsync(
                        transaction.UUID);

            if (consent == null)
            {
                throw new Exception(
                    "Consent record not found.");
            }

            response.ConsentRequired = true;

            response.ConsentStatus =
                consent.ConsentStatus;

            switch (consent.ConsentStatus)
            {
                case ConsentStatusConstants.Pending:

                    response.IsCompleted = false;

                    response.Message =
                        "Waiting for consent approval.";

                    break;

                case ConsentStatusConstants.Approved:

                    response.IsCompleted = true;

                    response.Message =
                        "Consent approved.";

                    break;

                case ConsentStatusConstants.Rejected:

                    response.IsCompleted = true;

                    response.Message =
                        "Consent rejected.";

                    break;

                case ConsentStatusConstants.Expired:

                    response.IsCompleted = true;

                    response.Message =
                        "Consent request expired.";

                    break;

                case ConsentStatusConstants.Cancelled:

                    response.IsCompleted = true;

                    response.Message =
                        "Consent request cancelled.";

                    break;

                default:

                    response.IsCompleted = false;

                    response.Message =
                        consent.ConsentStatus;

                    break;
            }

            return response;
        }

        public async Task<TransactionSummaryDto?> GetByUuidAsync(
            string transactionUuid)
        {
            var transaction =
                await _transactionRepository
                    .GetByUuidAsync(
                        transactionUuid);

            if (transaction == null)
            {
                return null;
            }

            var consent =
                await _transactionRepository
                    .GetConsentByTransactionUuidAsync(
                        transactionUuid);

            return new TransactionSummaryDto
            {
                TransactionUUID =
                    transaction.UUID,

                TransactionNo =
                    transaction.TransactionNo,

                PaymentStatus =
                    transaction.PaymentStatus,

                PaymentMode =
                    transaction.PaymentMode,

                TotalPayableAmount =
                    transaction.TotalPayableAmount,

                ConsentStatus =
                    consent?.ConsentStatus
                    ?? "NA",

                IsConsentSentOnMobile =
                    consent?.IsConsentSentOnMobile
                    ?? false
            };
        }

        public async Task<ConsentPortalDto> GetConsentPortalAsync(
            string token)
        {
            var consent =
                await _transactionRepository
                    .GetConsentByTokenAsync(
                        token);

            if (consent == null)
            {
                throw new Exception(
                    "Invalid consent link.");
            }

            if (consent.IsExpired)
            {
                throw new Exception(
                    "Consent link expired.");
            }

            if (consent.ConsentExpiresAt <=
                DateTimeOffset.UtcNow)
            {
                throw new Exception(
                    "Consent link expired.");
            }

            var transaction =
                await _transactionRepository
                    .GetByUuidAsync(
                        consent.TransactionUUID);

            if (transaction == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            var verifier =
                await _masterCustomerRepository
                    .GetByUuidAsync(
                        transaction.VerifierUUID);

            if (verifier == null)
            {
                throw new Exception(
                    "Verifier not found.");
            }

            var details =
    await _transactionRepository
        .GetTransactionDetailsAsync(
            consent.TransactionUUID);

            var apiUuids =
                details
                    .Select(x => x.ApiUUID)
                    .Distinct()
                    .ToList();

            var apis =
                await _masterApiRepository
                    .GetByUuidsAsync(
                        apiUuids);

            var consentNotice =
                await _masterPolicyService
                    .GetByCodeAsync(
                        PolicyCodes.CONSENT_NOTICE);

            return new ConsentPortalDto
            {
                ConsentId =
                    consent.ConsentToken ?? "",

                TransactionUUID =
                    transaction.UUID,

                ConsentStatus =
                    consent.ConsentStatus,

                ConsentDateTime = consent.CreatedAt,

                VerifierName =
                    $"{verifier.FName} {verifier.LName}"
                        .Trim(),

                ConsentNotice =
                    consentNotice.PolicyContent,

                ConsentExpiresAt =
                    consent.ConsentExpiresAt,

                Documents =
    details
        .Select(detail =>
        {
            var api =
                apis.FirstOrDefault(x =>
                    x.UUID ==
                    detail.ApiUUID);
            // Extract documentNumber from ReqPl JSON: {"documentNumber":"..."}
            string documentNumber = string.Empty;

            if (!string.IsNullOrWhiteSpace(detail.ReqPl))
            {
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(detail.ReqPl);
                    var root = doc.RootElement;
                    if (root.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        var enumerator = root.EnumerateObject();
                        if (enumerator.MoveNext())
                        {
                            var firstProp = enumerator.Current;
                            var val = firstProp.Value;
                            if (val.ValueKind == System.Text.Json.JsonValueKind.String)
                            {
                                documentNumber = val.GetString() ?? string.Empty;
                            }
                            else
                            {
                                // For non-string values, use raw text
                                documentNumber = val.ToString();
                            }
                        }
                    }
                }
                catch
                {
                    // Ignore invalid JSON; leave documentNumber empty
                    documentNumber = string.Empty;
                }
            }
            return new ConsentDocumentDto
            {

                DocumentName =
                    api.ApiName,
                DocumentNumber =
                            documentNumber,
                IsConsentBased = api.IsConsentBased ?? false
            };
        })
        .ToList()
            };
        }

        public sealed class SendAadhaarOtpNewResponseDto
        {
            /// <summary>
            /// ClientId returned by Aadhaar provider.
            /// Required while verifying OTP.
            /// </summary>
            public string ClientId { get; set; }
                = string.Empty;
        }

        public sealed class ApproveConsentResponseDto
        {
            public bool IsConsentApproved { get; set; }
        }

        public async Task<SendAadhaarOtpNewResponseDto>
    SendAadhaarOtpAsync(
        string token)
        {
            var consent =
                await _transactionRepository
                    .GetConsentByTokenAsync(
                        token);

            if (consent == null)
            {
                throw new Exception(
                    "Invalid consent link.");
            }

            if (!consent.IsActive)
            {
                throw new Exception(
                    "Consent request is inactive.");
            }

            if (consent.IsExpired)
            {
                throw new Exception(
                    "Consent link has expired.");
            }

            if (consent.ConsentExpiresAt.HasValue &&
                consent.ConsentExpiresAt.Value <=
                DateTimeOffset.UtcNow)
            {
                consent.IsExpired = true;

                await _transactionRepository
                    .UpdateTransactionConsentAsync(
                        consent);

                await _transactionRepository
                    .SaveChangesAsync();

                throw new Exception(
                    "Consent link has expired.");
            }

            if (consent.ConsentStatus !=
                ConsentStatusConstants.Pending)
            {
                throw new Exception(
                    $"Consent is already {consent.ConsentStatus.ToLower()}.");
            }

            var transaction =
                await _transactionRepository
                    .GetByUuidAsync(
                        consent.TransactionUUID);

            if (transaction == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            if (!transaction.IsActive)
            {
                throw new Exception(
                    "Transaction is inactive.");
            }

            var aadhaarNumber =
                _encryptionService
                    .Decrypt(
                        consent.ConsentDocNo);

            if (string.IsNullOrWhiteSpace(
                    aadhaarNumber))
            {
                throw new Exception(
                    "Invalid Aadhaar number.");
            }

            // FIX Uncomment this in production after testing
            /*var otpResponse =
                await _aadhaarVerificationService
                    .SendOtpAsync(
                        aadhaarNumber);

            if (!otpResponse.Success)
            {
                throw new Exception(
                    otpResponse.Message);
            }*/

            consent.IsAadharOTPSent = true;
            // FIX Uncomment this in production after testing
            /*consent.AadharReqId =
                otpResponse.ClientId;*/

            await _transactionRepository
                .UpdateTransactionConsentAsync(
                    consent);

            await _transactionRepository
                .SaveChangesAsync();

            return new SendAadhaarOtpNewResponseDto
            {// FIX Uncomment this in production after testing
                ClientId = Utils.GetUUID()
                    //otpResponse.ClientId
            };
        }

        public async Task<ApproveConsentResponseDto> ApproveConsentAsync(
            string token,
            ApproveConsentRequestDto request)
        {
            var consent =
                await _transactionRepository
                    .GetConsentByTokenAsync(token);

            if (consent == null)
            {
                throw new Exception(
                    "Invalid consent link.");
            }

            if (!consent.IsActive)
            {
                throw new Exception(
                    "Consent request is inactive.");
            }

            if (consent.IsExpired)
            {
                throw new Exception(
                    "Consent link has expired.");
            }

            if (consent.ConsentExpiresAt.HasValue &&
                consent.ConsentExpiresAt.Value <= DateTimeOffset.UtcNow)
            {
                consent.IsExpired = true;

                await _transactionRepository
                    .UpdateTransactionConsentAsync(consent);

                await _transactionRepository
                    .SaveChangesAsync();

                throw new Exception(
                    "Consent link has expired.");
            }

            if (consent.ConsentStatus !=
                ConsentStatusConstants.Pending)
            {
                throw new Exception(
                    $"Consent is already {consent.ConsentStatus.ToLower()}.");
            }

            if (!consent.IsAadharOTPSent)
            {
                throw new Exception(
                    "Please send Aadhaar OTP first.");
            }

            if (string.IsNullOrWhiteSpace(request.ClientId))
            {
                throw new Exception(
                    "ClientId is required.");
            }

            var transaction =
                await _transactionRepository
                    .GetByUuidAsync(
                        consent.TransactionUUID);

            if (transaction == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            var aadhaarNumber =
                _encryptionService.Decrypt(
                    consent.ConsentDocNo);

            // FIX Uncomment this in production after testing
            /*var verifyResponse =
                await _aadhaarVerificationService
                    .VerifyOtpAsync(
                        aadhaarNumber,
                        request.ClientId,
                        request.AadhaarOtp);

            if (!verifyResponse.Success)
            {
                throw new Exception("Otp Verification failed!");
            }*/

            consent.ConsentStatus =
                ConsentStatusConstants.Approved;

            consent.ConsentSubmitterName =
                request.SubmitterName;

            consent.ConsentSubmittedTimestamp =
                DateTimeOffset.UtcNow;

            consent.IsAadharOTPSent = false;

            consent.AadharReqId = null;

            transaction.TransactionStatus =
                TransactionStatusConstants
                    .ReadyForExecution;

            await _transactionRepository
                .UpdateTransactionConsentAsync(
                    consent);

            await _transactionRepository
                .UpdateTransactionAsync(
                    transaction);

            await _transactionRepository
                .SaveChangesAsync();

            return new ApproveConsentResponseDto
            {
                IsConsentApproved = true
            };
        }

        public async Task RejectConsentAsync(
    string token,
    RejectConsentRequestDto request)
        {
            var consent =
                await _transactionRepository
                    .GetConsentByTokenAsync(token);

            if (consent == null)
            {
                throw new Exception(
                    "Invalid consent link.");
            }

            if (!consent.IsActive)
            {
                throw new Exception(
                    "Consent request is inactive.");
            }

            if (consent.IsExpired)
            {
                throw new Exception(
                    "Consent link has expired.");
            }

            if (consent.ConsentExpiresAt.HasValue &&
                consent.ConsentExpiresAt.Value <= DateTimeOffset.UtcNow)
            {
                consent.IsExpired = true;

                await _transactionRepository
                    .UpdateTransactionConsentAsync(
                        consent);

                await _transactionRepository
                    .SaveChangesAsync();

                throw new Exception(
                    "Consent link has expired.");
            }

            if (consent.ConsentStatus !=
                ConsentStatusConstants.Pending)
            {
                throw new Exception(
                    $"Consent is already {consent.ConsentStatus.ToLower()}.");
            }

            var transaction =
                await _transactionRepository
                    .GetByUuidAsync(
                        consent.TransactionUUID);

            if (transaction == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            consent.ConsentStatus =
                ConsentStatusConstants.Rejected;

            consent.ConsentSubmitterName =
                request.SubmitterName;

            consent.ConsentSubmittedTimestamp =
                DateTimeOffset.UtcNow;

            consent.IsAadharOTPSent = false;

            consent.AadharReqId = null;

            transaction.TransactionStatus =
                TransactionStatusConstants
                    .Cancelled;

            await _transactionRepository
                .UpdateTransactionConsentAsync(
                    consent);

            await _transactionRepository
                .UpdateTransactionAsync(
                    transaction);

            await _transactionRepository
                .SaveChangesAsync();
        }
    }
}
