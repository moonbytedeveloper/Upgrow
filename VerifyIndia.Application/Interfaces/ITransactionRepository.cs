using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(
            Transaction transaction);

        Task AddTransactionDetailAsync(
            TransactionDetail transactionDetail);

        Task AddTransactionConsentAsync(
            TransactionConsent transactionConsent);

        Task<Transaction?> GetByUuidAsync(
            string transactionUuid);

        Task<TransactionConsent?>
            GetConsentByTransactionUuidAsync(
                string transactionUuid);

        Task UpdateTransactionAsync(
            Transaction transaction);

        Task UpdateTransactionConsentAsync(
            TransactionConsent transactionConsent);

        Task SaveChangesAsync();

        Task<List<TransactionDetail>> GetTransactionDetailsAsync(
            string transactionUuid);

        Task<TransactionConsent?> GetConsentByTokenAsync(
            string token);

        Task<IDbContextTransaction> BeginTransactionAsync();

        Task<TransactionRzpPGRecord?> GetCurrentPaymentAsync(
            string transactionUuid);

        Task AddPaymentRecordAsync(
            TransactionRzpPGRecord entity);

        Task UpdatePaymentRecordAsync(
            TransactionRzpPGRecord entity);

        Task<List<TransactionRzpPGRecord>> GetPaymentHistoryAsync(
            string transactionUuid);

        Task<Transaction?> GetPaidTransactionByCartUuidAsync(
            string cartUuid);

        Task<List<Transaction>> GetActiveTransactionsByCartUuidAsync(
            string cartUuid);

        Task<bool> TryStartProcessingAsync(
            string transactionUuid);

        Task<bool> TryStartContinuationAsync(
            string transactionDetailUuid);

        Task<bool> TryStartRestartAsync(
            string transactionDetailUuid);

        Task MarkProcessingFailedAsync(
            string transactionUuid);

        Task UpdateTransactionDetailAsync(
            TransactionDetail transactionDetail);

        Task<TransactionDetail?> GetTransactionDetailByUuidAsync(
            string transactionDetailUuid);

        Task<TransactionDetail?> GetTransactionDetailAsync(
            string transactionUuid,
            string transactionDetailUuid);

    }
}
