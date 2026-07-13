using Microsoft.EntityFrameworkCore;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Infrastructure.Repositories.Master
{
    public class CustomerConsentRepository : ICustomerConsentRepository
    {
        private readonly AppDbContext _context;

        public CustomerConsentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerConsent?> GetByCustomerAndPolicyAsync(string customerUuid, string policyUuid, string policyVersion)
        {
            return await _context.CustomerConsent
                .FirstOrDefaultAsync(x => x.CustomerUUID == customerUuid
                    && x.PolicyUUID == policyUuid
                    && x.PolicyVersion == policyVersion);
        }

        public async Task<CustomerConsent?> GetLatestByCustomerUUIDAsync(string customerUuid)
        {
            return await _context.CustomerConsent
                .AsNoTracking()
                .Where(x =>
                    x.CustomerUUID == customerUuid &&
                    x.ConsentGiven)
                .OrderByDescending(x => x.SignedAt)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(CustomerConsent entity)
        {
            await _context.CustomerConsent.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CustomerConsent entity)
        {
            _context.CustomerConsent.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
