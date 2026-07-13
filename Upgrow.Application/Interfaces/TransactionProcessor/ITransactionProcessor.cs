using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Verification.Documents;
using Upgrow.Domain.Models;

namespace Upgrow.Application.Interfaces.TransactionProcessor
{
    public interface ITransactionProcessor
    {
        /*Task ProcessAsync(
            string transactionUuid,
            CancellationToken cancellationToken);*/

        Task<TransactionExecutionResult> ProcessAsync(
            string transactionUuid,
            CancellationToken cancellationToken);

    }
}
