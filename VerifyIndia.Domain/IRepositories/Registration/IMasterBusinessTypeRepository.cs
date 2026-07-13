using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Registration;

namespace VerifyIndia.Domain.IRepositories.Registration
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
