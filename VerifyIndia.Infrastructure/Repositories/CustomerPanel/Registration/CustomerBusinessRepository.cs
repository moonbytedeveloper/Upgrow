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
    public sealed class CustomerBusinessRepository
        : ICustomerBusinessRepository
    {
        private readonly AppDbContext
            _context;

        public CustomerBusinessRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerOrganization?>
            GetByUUIDAsync(
                string uuid)
        {
            return await _context
                .CustomerOrganization
                .FirstOrDefaultAsync(x =>
                    x.UUID == uuid
                    &&
                    x.IsActive);
        }

        public async Task<CustomerOrganization?>
            GetByCustomerUUIDAsync(
                string customerUuid)
        {
            return await _context
                .CustomerOrganization
                .FirstOrDefaultAsync(x =>
                    x.CustomerUUID == customerUuid
                    &&
                    x.IsActive);
        }

        public async Task<bool>
            ExistsAsync(
                string customerUuid)
        {
            return await _context
                .CustomerOrganization
                .AnyAsync(x =>
                    x.CustomerUUID == customerUuid
                    &&
                    x.IsActive);
        }

        public async Task AddAsync(
            CustomerOrganization entity)
        {
            await _context
                .CustomerOrganization
                .AddAsync(entity);

            await _context
                .SaveChangesAsync();
        }

        public async Task UpdateAsync(
            CustomerOrganization entity)
        {
            _context
                .CustomerOrganization
                .Update(entity);

            await _context
                .SaveChangesAsync();
        }

        public async Task<CustomerOrganization?> GetRequiredByCustomerUUIDAsync(
        string customerUuid)
        {
            var organization =
                await GetByCustomerUUIDAsync(
                    customerUuid);

            if (organization == null)
            {
                throw new Exception(
                    "Organization details not found.");
            }

            return organization;
        }
    }
}
