using Upgrow.Application.Commands.Auth;
using Upgrow.Application.DTO.Auth;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Common;

namespace Upgrow.Application.IServices.Auth
{
    public interface IWLLoginAttemptService : IMasterService<LoginAttemptDto, LoginAttemptCommand>
    {
        Task<PagedResult<LoginAttemptDto>> GetPagedByUserAsync(DataTableRequest request, string? userUuid);
    }
}