using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories.Master
{
    public interface IMasterCompanyBasicDataRepository : IMasterRepository<Master_CompanyBasicData>
    {
        Task<Master_CompanyBasicData?> GetFirstOrDefaultAsync();
    }
}
