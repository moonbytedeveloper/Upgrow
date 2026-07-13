using VerifyIndia.Application.Commands.Auth;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Application.IServices.Auth
{
    public interface ILoginLogoutService : IMasterService<LoginLogoutDto, LoginLogoutCommand>
    {
        Task<PagedResult<LoginLogoutDto>> GetPagedByUserAsync(DataTableRequest request, string? userUuid);
    }
}