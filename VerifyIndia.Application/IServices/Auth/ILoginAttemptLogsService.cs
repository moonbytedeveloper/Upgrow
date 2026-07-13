using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.IServices.Auth
{
    public interface ILoginAttemptLogsService
    {
        Task AddAsync(LoginAttemptDto dto);
    }
}