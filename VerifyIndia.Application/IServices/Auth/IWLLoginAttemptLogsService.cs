using Upgrow.Application.DTO.Auth;
using Upgrow.Domain.Common;

namespace Upgrow.Application.IServices.Auth
{
    public interface IWLLoginAttemptLogsService
    {
        Task AddAsync(LoginAttemptDto dto);
        Task<PagedResult<LoginAttemptDto>> GetPagedAsync(PaginationParams pagination, string userUuid, string? search = null);

        Task<PagedResult<LoginAttemptDto>> GetPagedByTenantAsync(PaginationParams pagination, string? tenantId, string? search = null);


    }
}