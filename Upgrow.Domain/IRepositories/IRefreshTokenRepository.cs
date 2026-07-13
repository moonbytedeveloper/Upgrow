using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Auth;

namespace Upgrow.Domain.IRepositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshTokens token);
        Task UpdateAsync(RefreshTokens token);
        Task<RefreshTokens?> GetByTokenAsync(string token);
        Task<List<RefreshTokens>> GetByCustomerAsync(string customerUuid);
        
    }
}
