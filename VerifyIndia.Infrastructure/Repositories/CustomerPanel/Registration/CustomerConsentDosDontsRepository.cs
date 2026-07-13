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
    public class CustomerConsentDosDontsRepository
        : ICustomerConsentDosDontsRepository
    {
        private readonly AppDbContext _context;

        public CustomerConsentDosDontsRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerConsentDosDonts?>
            GetByCustomerUUIDAsync(
                string customerUuid)
        {
            return await _context
                .CustomerConsentDosDonts
                .Where(x =>
                    x.CustomerUUID ==
                    customerUuid
                    &&
                    x.IsActive)
                .OrderByDescending(x =>
                    x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(
            CustomerConsentDosDonts entity)
        {
            ArgumentNullException.ThrowIfNull(
                entity);

            await _context
                .CustomerConsentDosDonts
                .AddAsync(
                    entity);

            await _context
                .SaveChangesAsync();
        }

        public async Task UpdateAsync(
            CustomerConsentDosDonts entity)
        {
            ArgumentNullException.ThrowIfNull(
                entity);

            _context
                .CustomerConsentDosDonts
                .Update(
                    entity);

            await _context
                .SaveChangesAsync();
        }

        public async Task<bool> HasAcceptedDocumentAsync(
            string customerUuid,
            string documentUuid)
        {
            return await _context
                .CustomerConsentDosDonts
                .AnyAsync(x =>
                    x.CustomerUUID ==
                    customerUuid
                    &&
                    x.DocumentUUID ==
                    documentUuid
                    &&
                    x.ConsentGiven
                    &&
                    x.IsActive);
        }
    }
}
