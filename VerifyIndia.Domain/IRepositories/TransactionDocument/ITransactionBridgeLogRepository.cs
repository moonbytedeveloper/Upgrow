using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories.TransactionDocument
{
    public interface ITransactionBridgeLogRepository
    {
        Task AddAsync(
            TransactionBridgeLog entity);

        Task<int> GetNextAttemptNoAsync(
            string transactionDetailUuid);

        Task<List<TransactionBridgeLog>> GetByTransactionUuidAsync(
            string transactionUuid);

        Task SaveChangesAsync();
    }
}
