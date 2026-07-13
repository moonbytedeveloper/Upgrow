using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification.Documents;

namespace VerifyIndia.Application.Verification.Interfaces
{
    public interface ITransactionExecutionService
    {
        Task<TransactionExecutionResult> ProcessAsync(
            string transactionUuid,
            CancellationToken cancellationToken);
    }
}
