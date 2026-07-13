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
    public class WLMasterMenuRepository : MasterRepositoryBase<WL_MasterMenu>, IWLMasterMenuRepository
    {
        public WLMasterMenuRepository(AppDbContext context) : base(context) { }
        public async Task<List<WL_MasterMenu>> GetMainParentAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.IsActive==true && x.IsParent ==true && x.MenuLevel == 1)
                .OrderBy(x => x.Sequence)
                .ToListAsync();
        }
        public async Task<List<WL_MasterMenu>> GetSubParentAsync(string mainParentUuid)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.IsActive == true && x.IsParent == true && x.MainParentUUID == mainParentUuid && x.MenuLevel == 2)
                .OrderBy(x => x.Sequence)
                .ToListAsync();
        }
    }
}
