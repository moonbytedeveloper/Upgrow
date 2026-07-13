using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.WL;
using Microsoft.EntityFrameworkCore;
using Upgrow.Domain.IRepositories.Master;
using Upgrow.Domain.IRepositories.WL;

namespace Upgrow.Infrastructure.Repositories.Master.WL
{
    public class WLPasswordPolicyRepository : MasterRepositoryBase<WL_PasswordPolicy>, IWLPasswordPolicyRepository
    {
        public WLPasswordPolicyRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<WL_PasswordPolicy?> GetFirstOrDefaultAsync()
        {
            return await _dbSet.Where(x => x.IsActive == true).FirstOrDefaultAsync();
        }
    }
}
