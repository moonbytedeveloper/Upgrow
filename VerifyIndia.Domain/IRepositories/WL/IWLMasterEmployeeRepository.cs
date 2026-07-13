using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL.Master;

namespace VerifyIndia.Domain.IRepositories.WL
{
    public interface IWLMasterEmployeeRepository : IMasterRepository<WL_MasterEmployee>
    {
        Task<WL_MasterEmployee?> GetByCredentialsAsync(string username);
        Task<WL_MasterEmployee?> GetByEmailAsync(string email);


        Task<WL_MasterEmployee?> GetByEmployeeCodeAsync(string employeeCode);
    }
}
