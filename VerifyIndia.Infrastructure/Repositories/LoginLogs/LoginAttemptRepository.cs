using Microsoft.EntityFrameworkCore;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;
using Upgrow.Infrastructure.Extensions;

namespace Upgrow.Infrastructure.Repositories
{
    public class LoginAttemptRepository : ILoginAttemptRepository
    {
        private readonly AppDbContext _context;

        public LoginAttemptRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LoginAttempts attempt)
        {
            await _context.LoginAttempts.AddAsync(attempt);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]?> GetLastCurrentHashAsync()
        {
            return await _context.LoginAttempts
                .OrderByDescending(x => x.Id)
                .Select(x => x.CurrentHash)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<LoginAttempts>> GetPagedRawAsync(PaginationParams pagination)
        {
            var query = _context.LoginAttempts
                .AsNoTracking()
                .OrderByDescending(x => x.Id);

            return await query.ToPagedResultAsync(pagination);
        }
    }
}