using AuthenticateIndia.Shared.Constants;
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
    public class CustomerRegDocumentRepository
        : ICustomerRegDocumentRepository
    {
        private readonly AppDbContext _context;

        public CustomerRegDocumentRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerRegDocument?>
            GetAadhaarDocumentAsync(
                string customerUuid)
        {
            return await _context
                .CustomerRegDocument
                .FirstOrDefaultAsync(x =>
                    x.CustomerUUID ==
                        customerUuid
                    && x.RecordCategory ==
                        CustomerRegDocumentType.AADHAAR
                    && x.IsActive);
        }

        public async Task AddAsync(
            CustomerRegDocument entity, bool saveChanges = true)
        {
            await _context.CustomerRegDocument
                .AddAsync(entity);

            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CustomerRegDocument?>
            GetByUUIDAsync(
                string uuid)
        {
            return await _context
                .CustomerRegDocument
                .FirstOrDefaultAsync(
                    x => x.UUID == uuid);
        }

        public async Task<CustomerRegDocument?>
            GetLatestByCustomerAsync(
                string customerUuid,
                string recordCategory)
        {
            return await _context
                .CustomerRegDocument
                .Where(x =>
                    x.CustomerUUID == customerUuid &&
                    x.RecordCategory == recordCategory)
                .OrderByDescending(
                    x => x.VerificationTimeStamp)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(
            CustomerRegDocument entity)
        {
            _context.CustomerRegDocument
                .Update(entity);

            await _context.SaveChangesAsync();
        }
    }
}
