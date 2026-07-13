using Upgrow.Application.DTO.Auth;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices.Auth
{
    public interface ILoginAttemptLogsService
    {
        Task AddAsync(LoginAttemptDto dto);
    }
}