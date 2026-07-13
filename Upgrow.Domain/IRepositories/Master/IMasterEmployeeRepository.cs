using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories.Master
{
    public interface IMasterEmployeeRepository : IMasterRepository<Master_Employee>
    {
        Task<Master_Employee?> GetByCredentialsAsync(string username);
        Task<Master_Employee?> GetByEmailAsync(string email);

       
        Task<Master_Employee?> GetByEmployeeCodeAsync(string employeeCode);

    }
}
