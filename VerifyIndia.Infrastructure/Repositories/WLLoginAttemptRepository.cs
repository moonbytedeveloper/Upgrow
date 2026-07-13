using Microsoft.EntityFrameworkCore;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Infrastructure.Extensions;

namespace VerifyIndia.Infrastructure.Repositories
{
    public class WLLoginAttemptRepository : IWLLoginAttemptRepository
    {
        private readonly AppDbContext _context;

        public WLLoginAttemptRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WL_LoginAttempts attempt)
        {
            await _context.WL_LoginAttempts.AddAsync(attempt);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]?> GetLastCurrentHashAsync()
        {
            return await _context.WL_LoginAttempts
                .OrderByDescending(x => x.Id)
                .Select(x => x.CurrentHash)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<WL_LoginAttempts>> GetPagedRawAsync(PaginationParams pagination)
        {
            var query = _context.WL_LoginAttempts
                .AsNoTracking()
                .OrderByDescending(x => x.Id);

            return await query.ToPagedResultAsync(pagination);
        }
    }
}