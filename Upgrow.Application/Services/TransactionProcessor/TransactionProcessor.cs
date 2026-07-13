using AuthenticateIndia.Shared.Constants.TransactionDocument;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.Common.Response;
using Upgrow.Application.Interfaces;
using Upgrow.Application.Interfaces.TransactionProcessor;
using Upgrow.Application.IServices.CustomerPanel;
using Upgrow.Application.Verification;
using Upgrow.Application.Verification.Documents;
using Upgrow.Application.Verification.Interfaces;
using Upgrow.Application.Verification.Verification;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.TransactionDocument;
using Upgrow.Domain.Models;

namespace Upgrow.Application.Services.TransactionProcessor
{
    public class TransactionProcessor
        : ITransactionProcessor
    {
        private readonly ITransactionStatusCalculator
            _transactionStatusCalculator;

        private readonly IVerificationApplicationService
            _verificationApplicationService;

        private readonly ITransactionBridgeLogRepository
            _transactionBridgeLogRepository;

        private readonly IVerificationRequestFactory
            _verificationRequestFactory;

        private readonly ITransactionRepository
            _transactionRepository;

        private readonly ILogger<TransactionProcessor>
            _logger;

        public TransactionProcessor(
            ITransactionStatusCalculator transactionStatusCalculator,
            IVerificationApplicationService verificationApplicationService,
            ITransactionBridgeLogRepository transactionBridgeLogRepository,
            IVerificationRequestFactory verificationRequestFactory,
            ITransactionRepository transactionRepository,
            ILogger<TransactionProcessor> logger)
        {
            _transactionStatusCalculator =
                transactionStatusCalculator;

            _verificationApplicationService =
                verificationApplicationService;

            _transactionBridgeLogRepository =
                transactionBridgeLogRepository;

            _verificationRequestFactory =
                verificationRequestFactory;

            _transactionRepository =
                transactionRepository;

            _logger =
                logger;
        }

        public async Task<TransactionExecutionResult> ProcessAsync(
    string transactionUuid,
    CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Processing transaction {TransactionUuid}",
                transactionUuid);

            var transaction =
                await _transactionRepository
                    .GetByUuidAsync(
                        transactionUuid);

            if (transaction == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            var details =
                await _transactionRepository
                    .GetTransactionDetailsAsync(
                        transactionUuid);

            if (!details.Any())
            {
                throw new Exception(
                    "Transaction details not found.");
            }

            var context =
                new TransactionProcessContext
                {
                    Transaction =
                        transaction,

                    Details =
                        details,

                    CancellationToken =
                        cancellationToken
                };

            transaction.ProcessingStatus =
                TransactionProcessingStatusConstants
                    .Processing;

            await _transactionRepository
                .UpdateTransactionAsync(
                    transaction);

            await _transactionRepository
                .SaveChangesAsync();

            var executionResults =
                new List<TransactionExecutionDetailResult>();

            foreach (var detail in context.Details)
            {
                if (detail.ProcessingStatus ==
                    TransactionDetailProcessingStatusConstants.Completed)
                {
                    continue;
                }

                if (detail.ProcessingStatus ==
                    TransactionDetailProcessingStatusConstants
                        .WaitingForCustomerInput)
                {
                    continue;
                }

                var result =
                    await ExecuteDetailAsync(
                        context,
                        detail);

                executionResults.Add(result);
            }

            await _transactionBridgeLogRepository
                .SaveChangesAsync();

            _transactionStatusCalculator.Recalculate(
                transaction,
                context.Details);

            transaction.CompletedAt =
                DateTimeOffset.UtcNow;

            await _transactionRepository
                .UpdateTransactionAsync(
                    transaction);

            await _transactionRepository
                .SaveChangesAsync();

            _logger.LogInformation(
                "Completed transaction processing {TransactionUuid}",
                transactionUuid);

            return new TransactionExecutionResult
            {
                TransactionUUID =
                    transaction.UUID,

                ProcessingStatus =
                    transaction.ProcessingStatus,

                Details =
                    executionResults
            };
        }

        private async Task<TransactionExecutionDetailResult> ExecuteDetailAsync(
    TransactionProcessContext context,
    TransactionDetail detail)
        {
            var request =
                _verificationRequestFactory
                    .CreateRequest(
                        detail.CurrentVerificationCode,
                        detail.ReqPl);

            var verificationContext =
                new VerificationContext
                {
                    VerificationCode =
                        detail.CurrentVerificationCode,

                    Request =
                        request
                };

            var verificationResult =
                await ExecuteStepAsync(
                    context,
                    detail,
                    verificationContext);

            if (verificationResult.RequiresUserInput)
            {
                detail.CurrentVerificationCode =
                    verificationResult.NextVerificationCode!;

                detail.ProcessingStatus =
                    TransactionDetailProcessingStatusConstants
                        .WaitingForCustomerInput;

                await _transactionRepository
                    .UpdateTransactionDetailAsync(
                        detail);

                var sendOtpResponse =
                    verificationContext.Response
                        as AadharSendOtpResponse;

                return new TransactionExecutionDetailResult
                {
                    TransactionDetailUuid =
                        detail.UUID,

                    CurrentVerificationCode =
                        detail.VerificationCode,

                    IsSuccess = true,

                    RequiresUserInput = true,

                    NextVerificationCode =
                        verificationContext.NextVerificationCode,

                    ClientId =
                        sendOtpResponse?.client_id,

                    CanResend = true,

                    OtpExpiryInSeconds = 600,

                    Message =
                        verificationContext.ProviderMessage
                };
            }

            detail.ProcessingStatus =
                verificationResult.IsSuccess
                    ? TransactionDetailProcessingStatusConstants.Completed
                    : TransactionDetailProcessingStatusConstants.Failed;

            // Reset to purchased verification after successful completion.
            detail.CurrentVerificationCode =
                detail.VerificationCode;

            await _transactionRepository
                .UpdateTransactionDetailAsync(
                    detail);

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
                    verificationContext.ProviderMessage
            };
        }

        private async Task<VerificationResult> ExecuteStepAsync(
    TransactionProcessContext transactionContext,
    TransactionDetail detail,
    VerificationContext verificationContext)
        {
            var requestDateTime =
                DateTimeOffset.UtcNow;

            var stopwatch =
                Stopwatch.StartNew();

            Exception? exception = null;

            var verificationResult =
                new VerificationResult
                {
                    IsSuccess = false
                };

            try
            {
                verificationResult =
                    await _verificationApplicationService
                        .ExecuteAsync(
                            verificationContext,
                            transactionContext.Transaction,
                            detail,
                            transactionContext.CancellationToken);
            }
            catch (Exception ex)
            {
                exception = ex;

                verificationContext.ProviderMessage =
                    ex.Message;

                verificationContext.IsProviderSuccess =
                    false;

                _logger.LogError(
                    ex,
                    "Verification failed for {VerificationCode}",
                    verificationContext.VerificationCode);
            }
            finally
            {
                stopwatch.Stop();

                await SaveBridgeLogAsync(
                    transactionContext,
                    detail,
                    verificationContext,
                    exception,
                    requestDateTime,
                    stopwatch.Elapsed);
            }

            return verificationResult;
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

        
    }
}
