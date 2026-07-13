using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Infrastructure.Repositories
{
    public class AppSettingRepository : IAppSettingRepository
    {
        private readonly AppDbContext _context;

        public AppSettingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> GetValueAsync(string key)
        {
            return await _context.AppSetting
                .Where(x =>
                    x.IsActive &&
                    x.Key == key)
                .Select(x => x.Value)
                .FirstOrDefaultAsync();
        }
    }
}
