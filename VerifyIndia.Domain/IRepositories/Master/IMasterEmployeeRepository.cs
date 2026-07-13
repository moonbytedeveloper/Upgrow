using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories.Master
{
    public interface IMasterEmployeeRepository : IMasterRepository<Master_Employee>
    {
        Task<Master_Employee?> GetByCredentialsAsync(string username);
        Task<Master_Employee?> GetByEmailAsync(string email);

       
        Task<Master_Employee?> GetByEmployeeCodeAsync(string employeeCode);

    }
}
