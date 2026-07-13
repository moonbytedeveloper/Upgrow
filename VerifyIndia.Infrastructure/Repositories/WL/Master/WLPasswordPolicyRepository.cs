using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL;
using Microsoft.EntityFrameworkCore;
using VerifyIndia.Domain.IRepositories.Master;
using VerifyIndia.Domain.IRepositories.WL;

namespace VerifyIndia.Infrastructure.Repositories.Master.WL
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
