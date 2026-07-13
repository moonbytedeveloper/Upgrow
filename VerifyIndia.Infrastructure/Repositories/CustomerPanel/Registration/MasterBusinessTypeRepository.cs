using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Registration;
using VerifyIndia.Domain.IRepositories.Registration;

namespace VerifyIndia.Infrastructure.Repositories.CustomerPanel.Registration
{
    public class MasterBusinessTypeRepository : IMasterBusinessTypeRepository
    {
        private readonly AppDbContext _context;

        public MasterBusinessTypeRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Master_BusinessType>>
            GetAllActiveAsync()
        {
            return await _context
                .Master_BusinessType
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Master_BusinessType?>
            GetByUUIDAsync(
                string uuid)
        {
            return await _context
                .Master_BusinessType
                .FirstOrDefaultAsync(x =>
                    x.UUID == uuid &&
                    x.IsActive);
        }

        public async Task<Master_BusinessType?>
            GetByCodeAsync(
                string code)
        {
            return await _context
                .Master_BusinessType
                .FirstOrDefaultAsync(x =>
                    x.Code == code &&
                    x.IsActive);
        }
    }
}
