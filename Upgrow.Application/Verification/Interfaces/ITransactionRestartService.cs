using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Verification.Documents;

namespace Upgrow.Application.Verification.Interfaces
{
    public interface ITransactionRestartService
    {
        Task<TransactionExecutionDetailResult> RestartAsync(
            string transactionUuid,
            string transactionDetailUuid,
            CancellationToken cancellationToken);
    }
}
