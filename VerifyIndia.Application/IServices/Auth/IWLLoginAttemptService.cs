using VerifyIndia.Application.Commands.Auth;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Application.IServices.Auth
{
    public interface IWLLoginAttemptService : IMasterService<LoginAttemptDto, LoginAttemptCommand>
    {
        Task<PagedResult<LoginAttemptDto>> GetPagedByUserAsync(DataTableRequest request, string? userUuid);
    }
}