using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Transaction;
using static VerifyIndia.Application.Services.CustomerPanel.TransactionService;

namespace VerifyIndia.Application.IServices
{
    public interface ITransactionService
    {
        Task<InitiateTransactionResponseDto>
            InitiateTransactionAsync(
                string verifierUuid,
                InitiateTransactionRequest request);

        Task<GenerateConsentLinkResponseDto>
            GenerateConsentLinkAsync(
                string transactionUuid);

        Task<SendConsentLinkResponseDto>
            SendConsentLinkAsync(
                string transactionUuid);

        Task<GetConsentStatusResponseDto> GetConsentStatusAsync(
            string verifierUuid,
            string transactionUuid);

        Task<TransactionSummaryDto?>
            GetByUuidAsync(
                string transactionUuid);

        Task<ConsentPortalDto> GetConsentPortalAsync(
            string token);

        Task<SendAadhaarOtpNewResponseDto>
    SendAadhaarOtpAsync(
        string token);

        Task<ApproveConsentResponseDto> ApproveConsentAsync(
            string token,
            ApproveConsentRequestDto request);

        Task RejectConsentAsync(
            string token,
            RejectConsentRequestDto request);

        Task<CreatePaymentOrderResponseDto> CreatePaymentOrderAsync(
            string transactionUuid);

        Task<VerifyPaymentResponseDto> VerifyPaymentAsync(
            VerifyPaymentRequest request);
    }
}
