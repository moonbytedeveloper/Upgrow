using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Verification.Documents;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.Verification.Interfaces
{
    public interface IVerificationExecutionService
    {
        Task<TransactionExecutionDetailResult> ExecuteAsync(
            Transaction transaction,
            TransactionDetail transactionDetail,
            object request,
            CancellationToken cancellationToken);
    }
}
