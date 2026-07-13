using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories
{
    public interface IWLMasterCmsRepository :IMasterRepository<WL_MasterCMS>
    {
        Task<List<WL_MasterCMS>> GetByTenantAsync(int tenantId);
    }
}
