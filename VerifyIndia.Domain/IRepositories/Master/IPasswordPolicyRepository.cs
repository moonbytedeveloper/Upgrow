using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories.Master
{
    public interface IPasswordPolicyRepository : IMasterRepository<Password_Policy>
    {
        Task<Password_Policy?> GetFirstOrDefaultAsync();
    }
}
