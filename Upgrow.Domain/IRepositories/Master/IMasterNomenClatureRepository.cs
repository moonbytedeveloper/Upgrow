using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories.Master
{
    public interface IMasterNomenClatureRepository : IMasterRepository<Master_Nomenclature>
    {
        Task<int?> GetMaxNumberByModuleAsync(string moduleKey, int digits);
    }
}
