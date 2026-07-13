using Upgrow.Shared.Constants.TransactionDocument;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Transaction;
using Upgrow.Application.DTO.Verification.Common.Request;
using Upgrow.Application.Interfaces;
using Upgrow.Application.Interfaces.TransactionProcessor;
using Upgrow.Application.IServices.CustomerPanel;
using Upgrow.Application.Verification.Documents;
using Upgrow.Application.Verification.Interfaces;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.TransactionDocument;
using Upgrow.Domain.Models;
using static Upgrow.Application.Constants;

namespace Upgrow.Application.Services.TransactionProcessor
{
    public class TransactionContinuationService : ITransactionContinuationService
    {
        private readonly ITransactionBridgeLogRepository
            _transactionBridgeLogRepository;

        private readonly ITransactionStatusCalculator
            _transactionStatusCalculator;

        private readonly ITransactionRepository
            _transactionRepository;

        private readonly IVerificationApplicationService
            _verificationApplicationService;

        private readonly IVerificationRequestFactory
            _verificationRequestFactory;

        public TransactionContinuationService(
            ITransactionBridgeLogRepository transactionBridgeLogRepository,
            ITransactionStatusCalculator transactionStatusCalculator,
            ITransactionRepository transactionRepository,
            IVerificationApplicationService verificationApplicationService,
            IVerificationRequestFactory verificationRequestFactory)
        {
            _transactionBridgeLogRepository =
                transactionBridgeLogRepository;

            _transactionStatusCalculator =
                transactionStatusCalculator;

            _transactionRepository =
                transactionRepository;

            _verificationApplicationService =
                verificationApplicationService;

            _verificationRequestFactory =
                verificationRequestFactory;
        }

        private async Task SaveBridgeLogAsync(
    TransactionProcessContext transactionContext,
    TransactionDetail detail,
    VerificationContext verificationContext,
    Exception? exception,
    DateTimeOffset requestDateTime,
    TimeSpan elapsed)
        {
            var bridgeLog =
                new TransactionBridgeLog
                {
                    UUID =
                        Utils.GetUUID(),

                    TransactionUUID =
                        transactionContext
                            .Transaction
                            .UUID,

                    TransactionDetailUUID =
                        detail.UUID,

                    VerificationCode =
                        detail.VerificationCode,

                    ProviderCode =
                        verificationContext.Provider
                        ?? string.Empty,

                    AttemptNo =
                        await _transactionBridgeLogRepository
                            .GetNextAttemptNoAsync(
                                detail.UUID),

                    RequestPayload =
                        detail.ReqPl,

                    ResponseStatusCode =
                        exception == null
                            ? (int)verificationContext
                                .ProviderStatusCode
                            : StatusCodes
                                .Status500InternalServerError,

                    ResponseMessage =
                        exception == null
                            ? verificationContext
                                .ProviderMessage
                            : exception.Message,

                    PrimaryIdentifier =
                        verificationContext
                            .PrimaryIdentifier,

                    ProviderReferenceNo =
                        null,

                    RequestDateTime =
                        requestDateTime,

                    ResponseDateTime =
                        DateTimeOffset.UtcNow,

                    LoadTime =
                        (decimal)elapsed
                            .TotalMilliseconds,

                    IsSuccess =
                        exception == null
                        &&
                        verificationContext
                            .IsProviderSuccess,

                    IsActive =
                        true
                };

            await _transactionBridgeLogRepository
                .AddAsync(
                    bridgeLog);
        }

        public async Task<TransactionExecutionDetailResult> ContinueAsync(
            string transactionUuid,
            string transactionDetailUuid,
            ContinueVerificationRequest request,
            CancellationToken cancellationToken)
        {
            var transaction =
                await _transactionRepository
                    .GetByUuidAsync(
                        transactionUuid);

            if (transaction == null)
            {
                throw new InvalidOperationException(
                    "Transaction not found.");
            }

            if (transaction.ProcessingStatus !=
                TransactionProcessingStatusConstants.PendingUserAction)
            {
                throw new InvalidOperationException(
                    "Transaction is not waiting for customer input.");
            }

            var detail =
                await _transactionRepository
                    .GetTransactionDetailAsync(
                        transactionUuid,
                        transactionDetailUuid);

            if (detail == null)
            {
                throw new InvalidOperationException(
                    "Transaction detail not found.");
            }

            if (!detail.IsMultistageRoute)
            {
                throw new InvalidOperationException(
                    "Verification is not multi-stage.");
            }

            if (detail.ProcessingStatus !=
                TransactionDetailProcessingStatusConstants
                    .WaitingForCustomerInput)
            {
                throw new InvalidOperationException(
                    "Verification is not waiting for customer input.");
            }

            var started =
                await _transactionRepository
                    .TryStartContinuationAsync(
                        transactionDetailUuid);

            if (!started)
            {
                throw new InvalidOperationException(
                    "Verification is already being processed.");
            }

            detail.ProcessingStatus =
                TransactionDetailProcessingStatusConstants
                    .Processing;

            object requestModel;

            switch (detail.CurrentVerificationCode)
            {
                case VerificationCodes.AadhaarVerifyOtp:

                    requestModel =
                        new AadhaarVerifyOtpRequest
                        {
                            client_id =
                                request.ClientId,

                            otp =
                                request.Otp
                        };

                    break;

                default:

                    throw new NotSupportedException(
                        $"Continue is not supported for verification '{detail.CurrentVerificationCode}'.");
            }

            var verificationContext =
                new VerificationContext
                {
                    VerificationCode =
                        detail.CurrentVerificationCode,

                    Request =
                        requestModel
                };

            var requestDateTime = DateTimeOffset.UtcNow;

            var stopwatch =
                Stopwatch.StartNew();

            Exception? exception = null;

            VerificationResult verificationResult;

            try
            {
                verificationResult =
                    await _verificationApplicationService
                        .ExecuteAsync(
                            verificationContext,
                            transaction,
                            detail,
                            cancellationToken);
            }
            catch (Exception ex)
            {
                detail.ProcessingStatus =
                    TransactionDetailProcessingStatusConstants
                        .WaitingForCustomerInput;

                await _transactionRepository
                    .UpdateTransactionDetailAsync(
                        detail);

                await _transactionRepository
                    .SaveChangesAsync();

                exception = ex;

                throw;
            }
            finally
            {
                stopwatch.Stop();

                await SaveBridgeLogAsync(
                    new TransactionProcessContext
                    {
                        Transaction = transaction
                    },
                    detail,
                    verificationContext,
                    exception,
                    requestDateTime,
                    stopwatch.Elapsed);

                await _transactionBridgeLogRepository
                    .SaveChangesAsync();
            }

            if (verificationResult.RequiresUserInput)
            {
                throw new InvalidOperationException(
                    "Verification still requires customer input.");
            }

            detail.ProcessingStatus =
                verificationResult.IsSuccess
                    ? TransactionDetailProcessingStatusConstants.Completed
                    : TransactionDetailProcessingStatusConstants.Failed;

            detail.CurrentVerificationCode = detail.VerificationCode;

            await _transactionRepository
                .UpdateTransactionDetailAsync(
                    detail);

            var details =
                await _transactionRepository
                    .GetTransactionDetailsAsync(
                        transactionUuid);

            _transactionStatusCalculator
                .Recalculate(
                    transaction,
                    details);

            if (transaction.ProcessingStatus ==
                TransactionProcessingStatusConstants.Completed)
            {
                transaction.CompletedAt =
                    DateTimeOffset.UtcNow;
            }

            await _transactionRepository
                .UpdateTransactionAsync(
                    transaction);

            await _transactionRepository
                .SaveChangesAsync();

            return new TransactionExecutionDetailResult
            {
                TransactionDetailUuid =
                    detail.UUID,

                CurrentVerificationCode =
                    detail.VerificationCode,

                IsSuccess =
                    verificationResult.IsSuccess,

                /*Response =
                    verificationContext.Response,*/

                DocumentBase64 =
                    verificationResult.DocumentBase64,

                Message =
                    verificationContext.ProviderMessage,

                RequiresUserInput =
                    verificationResult.RequiresUserInput,

                NextVerificationCode =
                    verificationResult.InputType
            };
        }
    }
}
