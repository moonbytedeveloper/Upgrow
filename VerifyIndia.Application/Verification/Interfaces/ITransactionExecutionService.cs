using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Verification.Documents;

namespace Upgrow.Application.Verification.Interfaces
{
    public interface ITransactionExecutionService
    {
        Task<TransactionExecutionResult> ProcessAsync(
            string transactionUuid,
            CancellationToken cancellationToken);
    }
}
