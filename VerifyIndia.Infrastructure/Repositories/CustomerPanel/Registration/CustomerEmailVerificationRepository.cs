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
    public class CustomerEmailVerificationRepository
        : ICustomerEmailVerificationRepository
    {
        private readonly AppDbContext _context;

        public CustomerEmailVerificationRepository(
            AppDbContext context)
        {
            _context =
                context;
        }

        public async Task<CustomerEmailVerification?> GetLatestAsync(
            string customerUuid)
        {
            return await _context
                .CustomerEmailVerification
                .Where(x =>
                    x.CustomerUUID ==
                    customerUuid)
                .OrderByDescending(x =>
                    x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(
            CustomerEmailVerification entity)
        {
            ArgumentNullException
                .ThrowIfNull(entity);

            await _context
                .CustomerEmailVerification
                .AddAsync(entity);

            await _context
                .SaveChangesAsync();
        }

        public async Task UpdateAsync(
            CustomerEmailVerification entity)
        {
            ArgumentNullException
                .ThrowIfNull(entity);

            _context
                .CustomerEmailVerification
                .Update(entity);

            await _context
                .SaveChangesAsync();
        }

        public async Task DeactivateActiveAsync(
            string customerUuid)
        {
            var records =
                await _context
                    .CustomerEmailVerification
                    .Where(x =>
                        x.CustomerUUID ==
                        customerUuid
                        &&
                        x.IsActive)
                    .ToListAsync();

            if (!records.Any())
            {
                return;
            }

            foreach (var record in records)
            {
                record.IsActive =
                    false;

                record.UpdatedAt =
                    DateTimeOffset.UtcNow;
            }

            await _context
                .SaveChangesAsync();
        }
    }
}
