using Microsoft.EntityFrameworkCore;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Infrastructure.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly AppDbContext _context;

        public ConfigurationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> GetFileDomainUrlAsync()
        {
            return await _context.Set<Configuration>()
                .AsNoTracking()
                .Where(x => !string.IsNullOrWhiteSpace(x.DomainUrl))
                .OrderBy(x => x.Id)
                .Select(x => x.DomainUrl)
                .FirstOrDefaultAsync();
        }
        public async Task<string?> GetSandboxAccessTokenAsync()
        {
            return await _context.Set<Configuration>()
                .AsNoTracking()
                .Where(x => !string.IsNullOrWhiteSpace(x.SandboxAccessToken))
                .OrderByDescending(x => x.Id)
                .Select(x => x.SandboxAccessToken)
                .FirstOrDefaultAsync();
        }
    }
}