using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Application.IServices.Auth
{
    public interface IWLLoginAttemptLogsService
    {
        Task AddAsync(LoginAttemptDto dto);
        Task<PagedResult<LoginAttemptDto>> GetPagedAsync(PaginationParams pagination, string userUuid, string? search = null);

        Task<PagedResult<LoginAttemptDto>> GetPagedByTenantAsync(PaginationParams pagination, string? tenantId, string? search = null);


    }
}