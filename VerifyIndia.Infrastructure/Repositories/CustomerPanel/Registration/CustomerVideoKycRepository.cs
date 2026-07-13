using AuthenticateIndia.Shared.Constants.Registration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Interfaces.Registration;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Infrastructure.Repositories.CustomerPanel.Registration
{
    public class CustomerVideoKycRepository
        : ICustomerVideoKycRepository
    {
        private readonly AppDbContext _context;

        public CustomerVideoKycRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            CustomerVideoKyc entity)
        {
            await _context
                .CustomerVideoKyc
                .AddAsync(entity);

            await _context
                .SaveChangesAsync();
        }

        public async Task UpdateAsync(
            CustomerVideoKyc entity)
        {
            _context
                .CustomerVideoKyc
                .Update(entity);

            await _context
                .SaveChangesAsync();
        }

        public async Task<CustomerVideoKyc?>
            GetLatestByCustomerUUIDAsync(
                string customerUuid)
        {
            return await _context
                .CustomerVideoKyc
                .AsNoTracking()
                .Where(x =>
                    x.CustomerUUID ==
                    customerUuid)
                .OrderByDescending(x =>
                    x.VerificationTimeStamp)
                .FirstOrDefaultAsync();
        }

        public async Task<bool>
            IsVerifiedAsync(
                string customerUuid)
        {
            return await _context
                .CustomerVideoKyc
                .AnyAsync(x =>
                    x.CustomerUUID ==
                    customerUuid &&
                    x.IsVerified);
        }

        public async Task<CustomerVideoKyc?> GetLatestChallengeAsync(
            string customerUuid)
        {
            return await _context
                .CustomerVideoKyc
                .Where(x =>
                    x.CustomerUUID ==
                    customerUuid && x.IsActive)
                .OrderByDescending(x =>
                    x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task DeactivateActiveChallengesAsync(
            string customerUuid)
        {
            var activeChallenges =
                await _context
                    .CustomerVideoKyc
                    .Where(x =>
                        x.CustomerUUID == customerUuid &&
                        x.IsActive)
                    .ToListAsync();

            foreach (var item in activeChallenges)
            {
                item.IsActive = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}
