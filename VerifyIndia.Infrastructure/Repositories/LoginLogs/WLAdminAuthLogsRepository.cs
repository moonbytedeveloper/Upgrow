using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.WL;
using VerifyIndia.Domain.IRepositories.LoginLogs;

namespace VerifyIndia.Infrastructure.Repositories.LoginLogs
{
    public class WLAdminAuthLogsRepository : IWLAdminAuthLogsRepository
    {
        private readonly AppDbContext _context;

        public WLAdminAuthLogsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(WL_AdminAuthLogs log)
        {
            await _context.WL_AdminAuthLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]?> GetLastCurrentHashAsync()
        {
            // Tenant filter is automatically applied by AppDbContext
            return await _context.WL_AdminAuthLogs
                .OrderByDescending(x => x.Id)
                .Select(x => x.CurrentHash)
                .FirstOrDefaultAsync();
        }
    }
}