using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories.Master
{
    public interface IWLMasterMenuRepository : IMasterRepository<WL_MasterMenu>
    {
        Task<List<WL_MasterMenu>> GetMainParentAsync();
        Task<List<WL_MasterMenu>> GetSubParentAsync(string mainParentUuid);
    }
}

