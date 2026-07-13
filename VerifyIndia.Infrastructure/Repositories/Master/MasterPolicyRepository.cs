using Microsoft.EntityFrameworkCore;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.Master;
using Upgrow.Application;

namespace Upgrow.Infrastructure.Repositories.Master
{
    public class MasterPolicyRepository : IMasterPolicyRepository
    {
        private readonly AppDbContext _context;

        public MasterPolicyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Master_Policy>> GetPolicyListAsync(List<string> codes)
        {
            return await _context.Master_Policy
                .AsNoTracking()
                .Where(x => codes.Contains(x.Code) && x.IsActive)
                .OrderBy(x => x.SequenceNo)
                .ThenBy(x => x.Title)
                .ToListAsync();
           
        }

        //public async Task<Master_Policy?> GetByUuidAsync(string uuid)
        //    => await _context.Master_Policy.FirstOrDefaultAsync(x => x.UUID == uuid);

        public async Task<Master_Policy?> GetByCodeAndVersionAsync(string code, string version)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(version))
                return null;

            var normalizedCode = code.Trim();
            var normalizedVersion = version.Trim();

            return await _context.Master_Policy
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == normalizedCode && x.Version == normalizedVersion);
        }
       

        public async Task<List<Master_Policy>> GetActivePrivacyAndTermsAsync()
        {
            var codes = new[] { Constants.PolicyCodes.PrivacyPolicy, Constants.PolicyCodes.TermsOfUse };
           
            return await _context.Master_Policy
               .AsNoTracking()
               .Where(x => x.IsActive == true
                           && x.Code != null
                           && (x.Code == Constants.PolicyCodes.PrivacyPolicy
                               || x.Code == Constants.PolicyCodes.TermsOfUse))
               .OrderByDescending(x => x.SequenceNo)
               .ToListAsync();

        }

        //public async Task<Master_Policy?> GetByCodeLatestAsync(string code)
        //{
        //    if (string.IsNullOrWhiteSpace(code))
        //        return null;

        //    return await _context.Master_Policy
        //        .AsNoTracking()
        //        .Where(x => x.Code == code && x.IsActive == true)
        //        .OrderByDescending(x => x.SequenceNo)
        //        .ThenByDescending(x => x.Version)
        //        .FirstOrDefaultAsync();
        //}
        public async Task<Master_Policy?> GetByCodeLatestAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            // Use case-sensitive collation for Code so entering "registration" will NOT match "REGISTRATION"
            const string caseSensitiveCollation = "SQL_Latin1_General_CP1_CS_AS";

            return await _context.Master_Policy
                .AsNoTracking()
                .Where(x => EF.Functions.Collate(x.Code, caseSensitiveCollation) == code && x.IsActive == true)
                .OrderByDescending(x => x.SequenceNo)
                .ThenByDescending(x => x.Version)
                .FirstOrDefaultAsync();
        }

        public async Task<Master_Policy?> GetByUuidAsync(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid)) return null;
            return await _context.Master_Policy.FirstOrDefaultAsync(x => x.UUID == uuid.Trim());
        }

        // NEW: Add entity
        public async Task AddAsync(Master_Policy entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _context.Master_Policy.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // NEW: Update entity
        public async Task UpdateAsync(Master_Policy entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _context.Master_Policy.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
