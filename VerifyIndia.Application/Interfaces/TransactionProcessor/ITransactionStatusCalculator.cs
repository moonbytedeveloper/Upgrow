using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.Interfaces.TransactionProcessor
{
    public interface ITransactionStatusCalculator
    {
        void Recalculate(
            Transaction transaction,
            IEnumerable<TransactionDetail> details);
    }
}
