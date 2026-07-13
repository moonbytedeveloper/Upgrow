using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories
{
    public interface IMasterApiRepository
    {
        Task<List<Master_Api>> GetByUuidsAsync(
            List<string> apiUuids);

        Task<Master_Api?> GetByUuidAsync(
            string apiUuid);
    }
}
