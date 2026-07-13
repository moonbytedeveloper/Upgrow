using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Infrastructure.Repositories.Master
{
    public class MasterEmailTemplateRepository : IMasterEmailTemplateRepository
    {
        private readonly AppDbContext _db;

        public MasterEmailTemplateRepository(AppDbContext dbContext)
        {
            _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Master_EmailTemplate?> GetByIdAsync(decimal id)
        {
            return await _db.Set<Master_EmailTemplate>()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
