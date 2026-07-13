using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.IServices.CustomerPanel
{
    public interface IVerificationApplicationService
    {
        Task<VerificationResult> ExecuteAsync(
            VerificationContext context,
            Transaction transaction,
            TransactionDetail detail,
            CancellationToken cancellationToken);
    }
}
