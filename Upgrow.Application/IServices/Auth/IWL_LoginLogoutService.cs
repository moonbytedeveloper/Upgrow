using Upgrow.Application.Commands.Auth;
using Upgrow.Application.DTO.Auth;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Common;

namespace Upgrow.Application.IServices.Auth
{
    public interface IWL_LoginLogoutService : IMasterService<LoginLogoutDto, WL_LoginLogoutCommand>
    {
        Task<PagedResult<LoginLogoutDto>> GetPagedByUserAsync(DataTableRequest request, string? userUuid);
    }
}