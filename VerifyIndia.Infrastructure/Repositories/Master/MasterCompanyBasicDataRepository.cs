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
    public class MasterComapnyBasicDataRepository : MasterRepositoryBase<Master_CompanyBasicData>, IMasterCompanyBasicDataRepository
    {
        public MasterComapnyBasicDataRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Master_CompanyBasicData?> GetFirstOrDefaultAsync()
        {
            return await _dbSet.Where(x => x.IsActive == true).FirstOrDefaultAsync();
        }
    }
}
