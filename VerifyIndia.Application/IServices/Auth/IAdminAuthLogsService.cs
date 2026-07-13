using Upgrow.Application.DTO.Auth;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices.Auth
{
    public interface IAdminAuthLogsService
    {
        Task AddAsync(AdminAuthLogDto dto);
    }
}