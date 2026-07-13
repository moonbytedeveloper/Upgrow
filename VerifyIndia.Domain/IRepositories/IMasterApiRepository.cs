using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories
{
    public interface IMasterApiRepository
    {
        Task<List<Master_Api>> GetByUuidsAsync(
            List<string> apiUuids);

        Task<Master_Api?> GetByUuidAsync(
            string apiUuid);
    }
}
