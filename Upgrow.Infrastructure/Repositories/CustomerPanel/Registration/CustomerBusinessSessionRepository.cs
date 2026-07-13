using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Registration;
using Upgrow.Domain.IRepositories.Registration;

namespace Upgrow.Infrastructure.Repositories.CustomerPanel.Registration
{
    public sealed class CustomerBusinessSessionRepository
        : ICustomerBusinessSessionRepository
    {
        private readonly AppDbContext
            _context;

        public CustomerBusinessSessionRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            CustomerBusinessSession session)
        {
            await _context
                .CustomerBusinessSession
                .AddAsync(session);

            await _context
                .SaveChangesAsync();
        }

        public async Task UpdateAsync(
            CustomerBusinessSession session)
        {
            _context
                .CustomerBusinessSession
                .Update(session);

            await _context
                .SaveChangesAsync();
        }

        public async Task<CustomerBusinessSession?>
            GetByUUIDAsync(
                string uuid)
        {
            return await _context
                .CustomerBusinessSession
                .FirstOrDefaultAsync(x =>
                    x.UUID == uuid);
        }

        public async Task<CustomerBusinessSession?>
            GetByOrderIdAsync(
                string orderId)
        {
            return await _context
                .CustomerBusinessSession
                .FirstOrDefaultAsync(x =>
                    x.RazorpayOrderId == orderId);
        }

        public async Task<CustomerBusinessSession?>
            GetActiveByCustomerUUIDAsync(
                string customerUuid)
        {
            return await _context
                .CustomerBusinessSession
                .FirstOrDefaultAsync(x =>
                    x.CustomerUUID == customerUuid
                    &&
                    x.IsActive);
        }

        public async Task DeactivateCustomerSessionsAsync(
            string customerUuid)
        {
            var sessions =
                await _context
                    .CustomerBusinessSession
                    .Where(x =>
                        x.CustomerUUID == customerUuid
                        &&
                        x.IsActive)
                    .ToListAsync();

            foreach (var session in sessions)
            {
                session.IsActive =
                    false;
            }

            await _context
                .SaveChangesAsync();
        }
    }
}
