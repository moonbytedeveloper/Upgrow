using AuthenticateIndia.Shared.Constants;
using AuthenticateIndia.Shared.Constants.TransactionDocument;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Interfaces;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories.CustomerPanel
{
    public class TransactionRepository :
        ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTransactionAsync(
            Transaction transaction)
        {
            await _context
                .Transactions
                .AddAsync(transaction);
        }

        public async Task AddTransactionDetailAsync(
            TransactionDetail transactionDetail)
        {
            await _context
                .TransactionDetails
                .AddAsync(transactionDetail);
        }

        public async Task AddTransactionConsentAsync(
            TransactionConsent transactionConsent)
        {
            await _context
                .TransactionConsents
                .AddAsync(transactionConsent);
        }

        public async Task<Transaction?>
            GetByUuidAsync(
                string transactionUuid)
        {
            return await _context
                .Transactions
                .FirstOrDefaultAsync(x =>
                    x.UUID == transactionUuid &&
                    x.IsActive);
        }

        public async Task<TransactionConsent?>
            GetConsentByTransactionUuidAsync(
                string transactionUuid)
        {
            return await _context
                .TransactionConsents
                .FirstOrDefaultAsync(x =>
                    x.TransactionUUID ==
                    transactionUuid &&
                    x.IsActive);
        }

        public Task UpdateTransactionAsync(
            Transaction transaction)
        {
            _context
                .Transactions
                .Update(transaction);

            return Task.CompletedTask;
        }

        public Task UpdateTransactionConsentAsync(
            TransactionConsent transactionConsent)
        {
            _context
                .TransactionConsents
                .Update(transactionConsent);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context
                .SaveChangesAsync();
        }

        public async Task<List<TransactionDetail>> GetTransactionDetailsAsync(
            string transactionUuid)
        {
            return await _context
                .TransactionDetails
                .Where(x =>
                    x.TransactionUUID ==
                    transactionUuid &&
                    x.IsActive)
                .ToListAsync();
        }

        public async Task<TransactionConsent?> GetConsentByTokenAsync(
            string token)
        {
            return await _context
                .TransactionConsents
                .FirstOrDefaultAsync(x =>
                    x.ConsentToken == token &&
                    x.IsActive);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database
                .BeginTransactionAsync();
        }

        public async Task<TransactionRzpPGRecord?> GetCurrentPaymentAsync(
            string transactionUuid)
        {
            return await _context
                .TransactionRzpPGRecords
                .FirstOrDefaultAsync(x =>
                    x.TransactionUUID ==
                        transactionUuid
                    &&
                    x.IsCurrent
                    &&
                    x.IsActive);
        }

        public async Task AddPaymentRecordAsync(
            TransactionRzpPGRecord entity)
        {
            await _context
                .TransactionRzpPGRecords
                .AddAsync(entity);
        }

        public Task UpdatePaymentRecordAsync(
            TransactionRzpPGRecord entity)
        {
            _context
                .TransactionRzpPGRecords
                .Update(entity);

            return Task.CompletedTask;
        }

        public async Task<List<TransactionRzpPGRecord>> GetPaymentHistoryAsync(
            string transactionUuid)
        {
            return await _context
                .TransactionRzpPGRecords
                .Where(x =>
                    x.TransactionUUID ==
                        transactionUuid
                    &&
                    x.IsActive)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetActiveTransactionsByCartUuidAsync(
        string cartUuid)
        {
            return await _context.Transactions
                .Where(x =>
                    x.CartUUID == cartUuid &&
                    x.IsActive)
                .ToListAsync();
        }

        public async Task<Transaction?> GetPaidTransactionByCartUuidAsync(
        string cartUuid)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(x =>
                    x.CartUUID == cartUuid &&
                    x.PaymentStatus ==
                        PaymentStatusConstants.Paid &&
                    x.IsActive);
        }

        public async Task<bool> TryStartProcessingAsync(
            string transactionUuid)
        {
            var affectedRows = await _context.Transactions
                .Where(x =>
                    x.UUID == transactionUuid &&
                    x.ProcessingStatus == TransactionProcessingStatusConstants.Pending)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(
                        x => x.ProcessingStatus,
                        TransactionProcessingStatusConstants.Processing));

            return affectedRows == 1;
        }

        public async Task<bool> TryStartContinuationAsync(
            string transactionDetailUuid)
        {
            var affectedRows =
                await _context.TransactionDetails
                    .Where(x =>
                        x.UUID == transactionDetailUuid &&
                        x.ProcessingStatus ==
                            TransactionDetailProcessingStatusConstants
                                .WaitingForCustomerInput)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(
                            x => x.ProcessingStatus,
                            TransactionDetailProcessingStatusConstants
                                .Processing));

            return affectedRows == 1;
        }

        public async Task<bool> TryStartRestartAsync(
            string transactionDetailUuid)
        {
            return await _context.TransactionDetails
                .Where(x =>
                    x.UUID == transactionDetailUuid
                    &&
                    x.ProcessingStatus ==
                        TransactionDetailProcessingStatusConstants
                            .WaitingForCustomerInput)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(
                        x => x.ProcessingStatus,
                        TransactionDetailProcessingStatusConstants
                            .Processing))
                > 0;
        }

        public async Task MarkProcessingFailedAsync(
    string transactionUuid)
        {
            var transaction =
                await GetByUuidAsync(
                    transactionUuid);

            if (transaction == null)
            {
                return;
            }

            transaction.ProcessingStatus =
                TransactionProcessingStatusConstants.Failed;

            transaction.CompletedAt =
                DateTimeOffset.UtcNow;

            await SaveChangesAsync();
        }

        public async Task<TransactionDetail?> GetTransactionDetailByUuidAsync(
            string transactionDetailUuid)
        {
            return await _context.TransactionDetails
                .FirstOrDefaultAsync(x =>
                    x.UUID == transactionDetailUuid &&
                    x.IsActive);
        }

        public async Task UpdateTransactionDetailAsync(
            TransactionDetail transactionDetail)
        {
            _context.TransactionDetails.Update(
                transactionDetail);

            await Task.CompletedTask;
        }

        public async Task<TransactionDetail?> GetTransactionDetailAsync(
            string transactionUuid,
            string transactionDetailUuid)
        {
            return await _context.TransactionDetails
                .FirstOrDefaultAsync(x =>
                    x.TransactionUUID ==
                        transactionUuid
                    &&
                    x.UUID ==
                        transactionDetailUuid
                    &&
                    x.IsActive);
        }
    }
}
