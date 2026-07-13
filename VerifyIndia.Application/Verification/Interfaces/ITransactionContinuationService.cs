using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Transaction;
using VerifyIndia.Application.Verification.Documents;

namespace VerifyIndia.Application.Verification.Interfaces
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
