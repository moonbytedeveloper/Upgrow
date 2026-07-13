using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Registration;
using VerifyIndia.Domain.IRepositories.Registration;

namespace VerifyIndia.Infrastructure.Repositories.CustomerPanel.Registration
{
    public sealed class CustomerOtpRepository
    : ICustomerOtpRepository
    {
        private readonly AppDbContext _context;

        public CustomerOtpRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerOtp?> GetLatestAsync(
            string customerUuid)
        {
            return await _context.CustomerOtp
                .Where(x => x.CustomerUUID == customerUuid)
                .OrderByDescending(x => x.CreatedAtUtc)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(
            CustomerOtp otp)
        {
            await _context.CustomerOtp.AddAsync(otp);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(
            CustomerOtp otp)
        {
            _context.CustomerOtp.Update(otp);

            await _context.SaveChangesAsync();
        }

        public async Task InvalidateExistingAsync(
            string customerUuid)
        {
            var records =
                await _context.CustomerOtp
                    .Where(x =>
                        x.CustomerUUID == customerUuid &&
                        !x.IsUsed)
                    .ToListAsync();

            foreach (var record in records)
            {
                record.IsUsed = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
