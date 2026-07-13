using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Infrastructure.Repositories
{
    public class LoginHistoryRepository : ILoginHistoryRepository
    {
        private readonly AppDbContext _context;

        public LoginHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DateTime?> GetLastLoginAsync(string userUuid)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
                return null;

            return await _context.Set<MasterUserLoginLog>()
                .AsNoTracking()
                .Where(x => x.EmployeeUUID == userUuid)
                .OrderByDescending(x => x.LoginAt)
                .Select(x => (DateTime?)x.LoginAt)
                .FirstOrDefaultAsync();
        }

        public async Task RecordLoginAsync(string userUuid, string ipAddress, DateTime loginAt)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
                throw new ArgumentException("userUuid is required", nameof(userUuid));

            var entry = new MasterUserLoginLog
            {
                EmployeeUUID = userUuid,
                IPAddress = ipAddress,
                LoginAt = loginAt,
                UUID = Guid.NewGuid().ToString()
            };

            await _context.Set<MasterUserLoginLog>().AddAsync(entry);
            await _context.SaveChangesAsync();
        }
    }
}
   
