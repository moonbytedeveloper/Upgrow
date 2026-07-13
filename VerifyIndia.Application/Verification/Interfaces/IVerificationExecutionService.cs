using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification.Documents;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.Verification.Interfaces
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
