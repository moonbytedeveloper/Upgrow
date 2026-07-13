using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Auth;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _db;

        public RefreshTokenRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(RefreshTokens token)
        {
            await _db.RefreshTokens.AddAsync(token);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateAsync(RefreshTokens token)
        {
            _db.RefreshTokens.Update(token);
            await _db.SaveChangesAsync();
        }

        public async Task<RefreshTokens?> GetByTokenAsync(string token)
        {
            return await _db.RefreshTokens
                .FirstOrDefaultAsync(x => x.RefreshToken == token);
        }

        public async Task<List<RefreshTokens>> GetByCustomerAsync(string customerUuid)
        {
            return await _db.RefreshTokens
                .Where(x => x.CustomerUUID == customerUuid)
                .ToListAsync();
        }     

       
    }
}