using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Auth;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Infrastructure.Repositories
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly AppDbContext _context;

        public PasswordResetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Auth_PassResetToken?> GetByTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            return await _context.Set<Auth_PassResetToken>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task AddAsync(Auth_PassResetToken tokenEntity)
        {
            await _context.Set<Auth_PassResetToken>().AddAsync(tokenEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Auth_PassResetToken tokenEntity)
        {
            _context.Set<Auth_PassResetToken>().Update(tokenEntity);
            await _context.SaveChangesAsync();
        }
    }
}
    
