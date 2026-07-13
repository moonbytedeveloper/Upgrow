using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Verification.Interfaces
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
