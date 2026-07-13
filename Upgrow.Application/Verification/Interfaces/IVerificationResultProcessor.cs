using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Models;

namespace Upgrow.Application.Verification.Interfaces
{
    public interface IVerificationResultProcessor
    {
        Task<VerificationResult> ProcessAsync(
            VerificationContext verificationContext,
            Transaction transaction,
            TransactionDetail transactionDetail,
            CancellationToken cancellationToken);
    }
}
