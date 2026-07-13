using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories
{
    public interface IWLMasterCmsRepository :IMasterRepository<WL_MasterCMS>
    {
        Task<List<WL_MasterCMS>> GetByTenantAsync(int tenantId);
    }
}
