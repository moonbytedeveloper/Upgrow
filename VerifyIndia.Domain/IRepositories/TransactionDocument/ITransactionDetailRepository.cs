using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories.TransactionDocument
{
    public interface ITransactionDetailRepository
    {
        Task<List<TransactionDetail>> GetByTransactionUuidAsync(
            string transactionUuid);
    }
}
