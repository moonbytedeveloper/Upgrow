using Microsoft.EntityFrameworkCore;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Infrastructure.Extensions;

namespace VerifyIndia.Infrastructure.Repositories.LoginLogs
{
    public sealed class LoginAttemptsRepository : ILoginAttemptRepository
    {
        private readonly AppDbContext _context;

        public LoginAttemptsRepository(AppDbContext context)
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
                .AsNoTracking()
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