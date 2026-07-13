using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification.Documents;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Interfaces.TransactionProcessor
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
