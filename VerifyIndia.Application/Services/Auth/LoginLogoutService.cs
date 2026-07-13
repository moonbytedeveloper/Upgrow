using System.Text.Json;
using VerifyIndia.Application.Commands.Auth;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.IRepositories.LoginLogs;

namespace VerifyIndia.Application.Services.Auth
{
    public sealed class LoginLogoutService : ILoginLogoutService
    {
        private readonly ILoginLogoutLogsRepository _repo;

        public LoginLogoutService(ILoginLogoutLogsRepository repo)
        {
            _repo = repo;
        }

   
        public Task<PagedResult<LoginLogoutDto>> GetPagedAsync(DataTableRequest request)
            => GetPagedByUserAsync(request, userUuid: null);

        public async Task<PagedResult<LoginLogoutDto>> GetPagedByUserAsync(DataTableRequest request, string? userUuid)
        {
            // Pull a reasonable window then filter/sort/page in memory.
            // (Same tradeoff style as ActivityLogsRepository: payload makes DB-side filtering hard.)
            var raw = await _repo.GetPagedRawAsync(new PaginationParams
            {
                PageNumber = 1,
                PageSize = 500
            });

            var rows = raw.Items
                .Select(x => Deserialize(x.Payload))
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();

            // Filter by userUuid (payload)
            if (!string.IsNullOrWhiteSpace(userUuid))
            {
                var u = userUuid.Trim();
                rows = rows.Where(x => string.Equals(x.UserUUID, u, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Search (payload)
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var s = request.Search.Trim();
                rows = rows.Where(x =>
                        (!string.IsNullOrWhiteSpace(x.Activity) && x.Activity.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(x.IPAddress) && x.IPAddress.Contains(s, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            // Sort (payload)
            rows = ApplySort(rows, request.SortColumn, request.SortDirection);

            var totalCount = rows.Count;

            // DataTables paging
            var take = request.Length <= 0 ? 10 : request.Length;
            var page = rows
                .Skip(request.Start)
                .Take(take)
                .Select(x => new LoginLogoutDto
                {
                    UserUUID = x.UserUUID,
                    Activity = string.IsNullOrWhiteSpace(x.Activity) ? "N/A" : x.Activity!,
                    CreatedAt = x.CreatedAt,
                    IpAddress = x.IPAddress ?? ""
                })
                .ToList();

            return new PagedResult<LoginLogoutDto>
            {
                Items = page,
                TotalCount = totalCount,
                PageNumber = (request.Start / take) + 1,
                PageSize = take
            };
        }

        // Read-only behavior (keeps interface satisfied)
        public Task SaveAsync(LoginLogoutCommand command, string userUuid, string ip, bool saveChanges = true)
            => throw new NotSupportedException("Login/logout logs are read-only.");
        public Task DeleteAsync(string uuid, string userUuid, string ip)
            => throw new NotSupportedException("Login/logout logs are read-only.");
        public Task<bool> ToggleActiveAsync(string uuid, string userUuid, string ip)
            => throw new NotSupportedException("Login/logout logs are read-only.");
        public Task<LoginLogoutDto?> GetByUuidAsync(string uuid) => Task.FromResult<LoginLogoutDto?>(null);
        public Task<List<MasterDropDownDto>> GetDropdownAsync(Func<LoginLogoutCommand, string> displaySelector)
            => Task.FromResult(new List<MasterDropDownDto>());

        private static AdminAuthLogDto? Deserialize(string? payload)
        {
            if (string.IsNullOrWhiteSpace(payload)) return null;
            try { return JsonSerializer.Deserialize<AdminAuthLogDto>(payload); }
            catch { return null; }
        }

        private static List<AdminAuthLogDto> ApplySort(List<AdminAuthLogDto> rows, string? sortColumn, string? sortDirection)
        {
            var asc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);

            return (sortColumn ?? "").Trim().ToLowerInvariant() switch
            {
                "date" or "createdat" => asc ? rows.OrderBy(x => x.CreatedAt).ToList()
                                             : rows.OrderByDescending(x => x.CreatedAt).ToList(),
                "activity" => asc ? rows.OrderBy(x => x.Activity).ToList()
                                  : rows.OrderByDescending(x => x.Activity).ToList(),
                "ip" or "ipaddress" => asc ? rows.OrderBy(x => x.IPAddress).ToList()
                                           : rows.OrderByDescending(x => x.IPAddress).ToList(),
                _ => rows
            };
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}