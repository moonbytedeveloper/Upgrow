using AuthenticateIndia.Shared.Constants;
using AuthenticateIndia.Shared.Constants.TransactionDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Common;
using Upgrow.Application.Interfaces;
using Upgrow.Application.Interfaces.TransactionProcessor;
using Upgrow.Application.Verification.Documents;
using Upgrow.Application.Verification.Interfaces;

namespace Upgrow.Application.Services.TransactionProcessor
{
    public class TransactionExecutionService
       : ITransactionExecutionService
    {
        private readonly AuditContext _auditContext;

        private readonly ITransactionRepository
            _transactionRepository;

        private readonly ITransactionProcessor
            _transactionProcessor;

        public TransactionExecutionService(
            AuditContext auditContext,
            ITransactionRepository transactionRepository,
            ITransactionProcessor transactionProcessor)
        {
            _auditContext = auditContext;

            _transactionRepository =
                transactionRepository;

            _transactionProcessor =
                transactionProcessor;
        }

        public async Task<TransactionExecutionResult> ProcessAsync(
        string transactionUuid,
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
                    "You are not authorized to execute this transaction.");
            }

            if (!string.Equals(
                    transaction.PaymentStatus,
                    PaymentStatusConstants.Paid,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Payment is not completed.");
            }

            if (string.Equals(
                    transaction.TransactionStatus,
                    TransactionStatusConstants.Cancelled,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Transaction has been cancelled.");
            }

            if (string.Equals(
                    transaction.TransactionStatus,
                    TransactionStatusConstants.ConsentPending,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Consent approval is pending.");
            }

            if (!string.Equals(
                    transaction.TransactionStatus,
                    TransactionStatusConstants.ReadyForExecution,
                    StringComparison.OrdinalIgnoreCase)
                &&
                !string.Equals(
                    transaction.TransactionStatus,
                    TransactionStatusConstants.ConsentApproved,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Transaction is not ready for execution.");
            }

            if (string.Equals(
                    transaction.ProcessingStatus,
                    TransactionProcessingStatusConstants.Completed,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Transaction has already been processed.");
            }

            if (string.Equals(
                    transaction.ProcessingStatus,
                    TransactionProcessingStatusConstants.PendingUserAction,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Transaction is waiting for customer input. Continue the pending verification instead.");
            }

            if (string.Equals(
                    transaction.ProcessingStatus,
                    TransactionProcessingStatusConstants.Processing,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Transaction is already being processed.");
            }

            var locked =
                await _transactionRepository
                    .TryStartProcessingAsync(
                        transactionUuid);

            if (!locked)
            {
                throw new InvalidOperationException(
                    "Transaction is already being processed.");
            }

            try
            {
                return await _transactionProcessor
                    .ProcessAsync(
                        transactionUuid,
                        cancellationToken);
            }
            catch
            {
                await _transactionRepository
                    .MarkProcessingFailedAsync(
                        transactionUuid);

                throw;
            }
        }
    }
}
