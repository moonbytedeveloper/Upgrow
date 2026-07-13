using Microsoft.EntityFrameworkCore;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;


namespace Upgrow.Infrastructure.Repositories
{
    public class AdminAuthLogsRepository : IAdminAuthLogsRepository
    {
        private readonly AppDbContext _context;

        public AdminAuthLogsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AdminAuthLogs log)
        {
            await _context.AdminAuthLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]?> GetLastCurrentHashAsync()
        {
            return await _context.AdminAuthLogs
                .OrderByDescending(x => x.Id)
                .Select(x => x.CurrentHash)
                .FirstOrDefaultAsync();
        }
    }
}