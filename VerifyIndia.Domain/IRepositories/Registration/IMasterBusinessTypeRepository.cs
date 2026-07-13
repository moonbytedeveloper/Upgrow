using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Domain.IRepositories.Registration
{
    public interface IMasterBusinessTypeRepository
    {
        Task<List<Master_BusinessType>> GetAllActiveAsync();

        Task<Master_BusinessType?> GetByUUIDAsync(
                string uuid);

        Task<Master_BusinessType?> GetByCodeAsync(
                string code);
    }
}
