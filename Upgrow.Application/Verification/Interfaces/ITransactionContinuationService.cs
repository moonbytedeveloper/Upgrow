using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Transaction;
using Upgrow.Application.Verification.Documents;

namespace Upgrow.Application.Verification.Interfaces
{
    public interface ITransactionContinuationService
    {
        Task<TransactionExecutionDetailResult> ContinueAsync(
            string transactionUuid,
            string transactionDetailUuid,
            ContinueVerificationRequest request,
            CancellationToken cancellationToken);
    }
}
