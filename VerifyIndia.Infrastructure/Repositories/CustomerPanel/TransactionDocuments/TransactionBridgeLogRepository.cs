using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.TransactionDocument;

namespace Upgrow.Infrastructure.Repositories.CustomerPanel.TransactionDocuments
{
    public class TransactionBridgeLogRepository
        : ITransactionBridgeLogRepository
    {
        private readonly AppDbContext
            _context;

        public TransactionBridgeLogRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            TransactionBridgeLog entity)
        {
            await _context.TransactionBridgeLog
                .AddAsync(entity);
        }

        public async Task<int> GetNextAttemptNoAsync(
            string transactionDetailUuid)
        {
            var lastAttempt =
                await _context.TransactionBridgeLog
                    .Where(x =>
                        x.TransactionDetailUUID ==
                        transactionDetailUuid)
                    .MaxAsync(x =>
                        (int?)x.AttemptNo);

            return (lastAttempt ?? 0) + 1;
        }

        public async Task<List<TransactionBridgeLog>> GetByTransactionUuidAsync(
            string transactionUuid)
        {
            return await _context.TransactionBridgeLog
                .Where(x =>
                    x.TransactionUUID ==
                    transactionUuid)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
