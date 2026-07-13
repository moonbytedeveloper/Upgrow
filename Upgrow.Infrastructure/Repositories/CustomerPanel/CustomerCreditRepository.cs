using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Interfaces;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Repositories.CustomerPanel
{
    public class CustomerCreditRepository :
        ICustomerCreditRepository
    {
        private readonly AppDbContext _context;

        public CustomerCreditRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerCreditMaster?>
            GetByCustomerUuidAsync(
                string customerUuid)
        {
            return await _context
                .CustomerCreditMasters
                .FirstOrDefaultAsync(x =>
                    x.CustomerUUID ==
                    customerUuid &&
                    x.IsActive);
        }

        public async Task AddCreditMasterAsync(
            CustomerCreditMaster entity)
        {
            await _context
                .CustomerCreditMasters
                .AddAsync(entity);
        }

        public async Task AddCreditLedgerAsync(
            CustomerCreditLedger entity)
        {
            await _context
                .CustomerCreditLedgers
                .AddAsync(entity);
        }

        public async Task AddDebitLedgerAsync(
            CustomerDebitLedger entity)
        {
            await _context
                .CustomerDebitLedgers
                .AddAsync(entity);
        }

        public Task UpdateCreditMasterAsync(
            CustomerCreditMaster entity)
        {
            _context
                .CustomerCreditMasters
                .Update(entity);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context
                .SaveChangesAsync();
        }

        public async Task<bool> HasSufficientCreditsAsync(
            string customerUuid,
            decimal requiredAmount)
        {
            var balance =
                await _context
                    .CustomerCreditMasters
                    .Where(x =>
                        x.CustomerUUID ==
                        customerUuid &&
                        x.IsActive)
                    .Select(x =>
                        x.CurrentBalance)
                    .FirstOrDefaultAsync();

            return balance >= requiredAmount;
        }

        public async Task<decimal?> GetCurrentBalanceAsync(string customerUuid)
        {
            return await _context
                .CustomerCreditMasters
                .Where(x => x.CustomerUUID == customerUuid && x.IsActive)
                .Select(x => (decimal?)x.CurrentBalance)
                .FirstOrDefaultAsync();
        }
    }
}
