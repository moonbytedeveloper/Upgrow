using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Infrastructure.Repositories.Master
{
    public class MasterEmailCredentialRepository :IMasterEmailCredentialRepository
    {
        private readonly AppDbContext _db;

        public MasterEmailCredentialRepository(AppDbContext dbContext)
        {
            _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Master_EmailCredential?> GetByUuidAsync(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid)) return null;

            return await _db.Set<Master_EmailCredential>()
                .FirstOrDefaultAsync(x => x.UUID == uuid);
        }

      
    }
}

    