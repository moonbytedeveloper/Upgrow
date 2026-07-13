using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification.Documents;

namespace VerifyIndia.Application.Verification.Interfaces
{
    public interface ITransactionRestartService
    {
        Task<TransactionExecutionDetailResult> RestartAsync(
            string transactionUuid,
            string transactionDetailUuid,
            CancellationToken cancellationToken);
    }
}
