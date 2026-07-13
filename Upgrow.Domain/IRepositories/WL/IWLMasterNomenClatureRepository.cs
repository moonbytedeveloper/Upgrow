using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.WL.Master;

namespace Upgrow.Domain.IRepositories.Master
{
    public interface IWLMasterNomenClatureRepository : IMasterRepository<WL_MasterNomenClature>
    {
        Task<int?> GetMaxNumberByModuleAsync(string moduleKey, int digits);
    }
}
