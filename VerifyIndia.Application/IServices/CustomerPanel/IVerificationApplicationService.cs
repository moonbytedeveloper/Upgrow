using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Models;

namespace Upgrow.Application.IServices.CustomerPanel
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
