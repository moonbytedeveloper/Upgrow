using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories.Master
{
    public interface IMasterNomenClatureRepository : IMasterRepository<Master_Nomenclature>
    {
        Task<int?> GetMaxNumberByModuleAsync(string moduleKey, int digits);
    }
}
