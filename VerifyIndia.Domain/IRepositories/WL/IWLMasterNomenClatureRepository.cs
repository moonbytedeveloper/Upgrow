using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL.Master;

namespace VerifyIndia.Domain.IRepositories.Master
{
    public interface IWLMasterNomenClatureRepository : IMasterRepository<WL_MasterNomenClature>
    {
        Task<int?> GetMaxNumberByModuleAsync(string moduleKey, int digits);
    }
}
