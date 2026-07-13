using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Registration;
using VerifyIndia.Domain.IRepositories.Registration;

namespace VerifyIndia.Infrastructure.Repositories.CustomerPanel.Registration
{
    public class CustomerAadhaarSessionRepository
        : ICustomerAadhaarSessionRepository
    {
        private readonly AppDbContext _context;

        public CustomerAadhaarSessionRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            CustomerAadhaarSession entity,
            bool saveChanges = true)
        {
            await _context
                .CustomerAadhaarSession
                .AddAsync(entity);

            if (saveChanges)
            {
                await _context
                    .SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(
            CustomerAadhaarSession entity,
            bool saveChanges = true)
        {
            _context
                .CustomerAadhaarSession
                .Update(entity);

            if (saveChanges)
            {
                await _context
                    .SaveChangesAsync();
            }
        }

        public async Task<CustomerAadhaarSession?>
            GetByUUIDAsync(
                string uuid)
        {
            return await _context
                .CustomerAadhaarSession
                .FirstOrDefaultAsync(
                    x => x.UUID == uuid);
        }

        public async Task<CustomerAadhaarSession?> GetActiveByCustomerUUIDAsync(
            string customerUuid)
        {
            return await _context.CustomerAadhaarSession
                .Where(x =>
                    x.CustomerUUID == customerUuid
                    &&
                    x.IsActive)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task DeactivateCustomerSessionsAsync(
            string customerUuid)
        {
            var sessions =
                await _context.CustomerAadhaarSession
                    .Where(x =>
                        x.CustomerUUID == customerUuid
                        &&
                        x.IsActive)
                    .ToListAsync();

            foreach (var session in sessions)
            {
                session.IsActive = false;
                session.UpdatedAt = DateTimeOffset.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasCompletedPaymentAsync(
            string customerUuid)
        {
            return await _context
                .CustomerAadhaarSession
                .AnyAsync(x =>
                    x.CustomerUUID == customerUuid
                    &&
                    x.IsPaymentCompleted);
        }
    }
}
