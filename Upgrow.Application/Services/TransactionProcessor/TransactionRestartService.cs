using AuthenticateIndia.Shared.Constants.TransactionDocument;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Common;
using Upgrow.Application.DTO.Verification.Common.Response;
using Upgrow.Application.Interfaces;
using Upgrow.Application.Interfaces.TransactionProcessor;
using Upgrow.Application.IServices.CustomerPanel;
using Upgrow.Application.Verification.Documents;
using Upgrow.Application.Verification.Interfaces;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.TransactionDocument;
using Upgrow.Domain.Models;

namespace Upgrow.Application.Services.TransactionProcessor
{
    public sealed class TransactionRestartService
    : ITransactionRestartService
    {
        private readonly AuditContext _auditContext;

        private readonly IVerificationStepResolver
            _verificationStepResolver;

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

        public TransactionRestartService(
            AuditContext auditContext,
            IVerificationStepResolver verificationStepResolver,
            ITransactionBridgeLogRepository transactionBridgeLogRepository,
            ITransactionStatusCalculator transactionStatusCalculator,
            ITransactionRepository transactionRepository,
            IVerificationApplicationService verificationApplicationService,
            IVerificationRequestFactory verificationRequestFactory)
        {
            _auditContext = auditContext;
            _verificationStepResolver =
                verificationStepResolver;

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

        public async Task<TransactionExecutionDetailResult> RestartAsync(
            string transactionUuid,
            string transactionDetailUuid,
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

            if (!string.Equals(
                transaction.VerifierUUID,
                _auditContext.CustomerUUID,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException(
                    "You are not authorized to restart this transaction.");
            }

            if (transaction.ProcessingStatus !=
                TransactionProcessingStatusConstants
                    .PendingUserAction)
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
                    .TryStartRestartAsync(
                        transactionDetailUuid);

            if (!started)
            {
                throw new InvalidOperationException(
                    "Verification is already being processed.");
            }

            var restartVerificationCode =
                _verificationStepResolver
                    .GetPreviousVerificationCode(
                        detail.CurrentVerificationCode);

            if (!string.Equals(
                restartVerificationCode,
                detail.VerificationCode,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Restart verification step is invalid.");
            }


            var requestModel =
                _verificationRequestFactory
                    .CreateRequest(
                        restartVerificationCode,
                        detail.ReqPl);

            var verificationContext =
                new VerificationContext
                {
                    VerificationCode =
                        restartVerificationCode,

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

                if (!verificationResult.RequiresUserInput)
                {
                    throw new InvalidOperationException(
                        "Restart did not produce another continuation step.");
                }
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

            detail.CurrentVerificationCode =
    verificationResult.NextVerificationCode!;

            detail.ProcessingStatus =
                TransactionDetailProcessingStatusConstants
                    .WaitingForCustomerInput;

            await _transactionRepository
                .UpdateTransactionDetailAsync(
                    detail);

            await _transactionRepository
                .SaveChangesAsync();

            var sendOtpResponse =
                verificationContext.Response
                    as AadharSendOtpResponse;

            return new TransactionExecutionDetailResult
            {
                TransactionDetailUuid =
                    detail.UUID,

                CurrentVerificationCode =
                    detail.CurrentVerificationCode,

                IsSuccess = true,

                RequiresUserInput = true,

                NextVerificationCode =
                    verificationResult.NextVerificationCode,

                ClientId =
                    sendOtpResponse?.client_id,

                CanResend = true,

                OtpExpiryInSeconds = 600,

                Message =
                    verificationContext.ProviderMessage
            };
        }
    }
}
