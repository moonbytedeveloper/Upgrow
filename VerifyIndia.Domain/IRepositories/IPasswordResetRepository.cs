using VerifyIndia.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.IRepositories
{
    public interface IPasswordResetRepository
    {
        Task<Auth_PassResetToken?> GetByTokenAsync(string token);
        Task AddAsync(Auth_PassResetToken tokenEntity);
        Task UpdateAsync(Auth_PassResetToken tokenEntity);
    }
}
