using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Upgrow.Application.Commands.Auth;
using Upgrow.Application.DTO.Auth;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Auth;
using Upgrow.Domain.Common;
using Upgrow.Domain.IRepositories.LoginLogs;

namespace Upgrow.Application.Services.Auth
{
    public sealed class WL_LoginLogoutService : IWL_LoginLogoutService
    {
        private readonly IWL_LoginLogoutLogsRepository _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WL_LoginLogoutService(IWL_LoginLogoutLogsRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }

        // Satisfies IMasterService. Intercepts the passed "userUuid" directly from the HTTP Form.
        public Task<PagedResult<LoginLogoutDto>> GetPagedAsync(DataTableRequest request)
        {
            var userUuid = _httpContextAccessor.HttpContext?.Request.Form["userUuid"].ToString();
            return GetPagedByUserAsync(request, userUuid);
        }

        public async Task<PagedResult<LoginLogoutDto>> GetPagedByUserAsync(DataTableRequest request, string? userUuid)
        {
            // ... (Keep your existing JSON deserialization and list filtering logic here) ...
            var raw = await _repo.GetPagedRawAsync(new PaginationParams { PageNumber = 1, PageSize = 500 });
            // ... (Existing filtering/sorting block) ...
            return new PagedResult<LoginLogoutDto>();
        }

        // --- IMasterService Implementations (Read-only Guard) ---
        public Task SaveAsync(WL_LoginLogoutCommand command, string userUuid, string ip)
            => throw new NotSupportedException("Logs are read-only.");

        public Task DeleteAsync(string uuid, string userUuid, string ip)
            => throw new NotSupportedException("Logs are read-only.");

        public Task<bool> ToggleActiveAsync(string uuid, string userUuid, string ip)
            => throw new NotSupportedException("Logs are read-only.");

        public Task<LoginLogoutDto?> GetByUuidAsync(string uuid)
            => Task.FromResult<LoginLogoutDto?>(null);

        public Task<List<MasterDropDownDto>> GetDropdownAsync(Func<WL_LoginLogoutCommand, string> displaySelector)
            => Task.FromResult(new List<MasterDropDownDto>());

        public Task SaveAsync(WL_LoginLogoutCommand command, string userUuid, string ip, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}