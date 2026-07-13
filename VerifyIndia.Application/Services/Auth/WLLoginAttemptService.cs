using System.Text.Json;
using VerifyIndia.Application.Commands.Auth;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Auth
{
    public sealed class WLLoginAttemptService : IWLLoginAttemptService
    {
        private readonly IWLLoginAttemptRepository _repo;

        public WLLoginAttemptService(IWLLoginAttemptRepository repo)
        {
            _repo = repo;
        }

        public Task<PagedResult<LoginAttemptDto>> GetPagedAsync(DataTableRequest request)
            => GetPagedByUserAsync(request, userUuid: null);

        public async Task<PagedResult<LoginAttemptDto>> GetPagedByUserAsync(DataTableRequest request, string? userUuid)
        {
            try
            {
                var pagination = new PaginationParams
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.Length > 0 ? request.Length : 10
                };

                // Get paginated raw data from repository (database-level pagination)
                var raw = await _repo.GetPagedRawAsync(pagination);

                // Deserialize all payloads
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
                            (!string.IsNullOrWhiteSpace(x.UserName) && x.UserName.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrWhiteSpace(x.FailureReason) && x.FailureReason.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrWhiteSpace(x.IpAddress) && x.IpAddress.Contains(s, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                // Sort (payload)
                rows = ApplySort(rows, request.SortColumn, request.SortDirection);

                var totalCount = rows.Count;

                // Apply DataTables paging to filtered results
                var take = pagination.PageSize;
                var skip = (pagination.PageNumber - 1) * take;
                var page = rows
                    .Skip(skip)
                    .Take(take)
                    .Select(x => new LoginAttemptDto
                    {
                        UserUUID = x.UserUUID,
                        UserName = x.UserName ?? "",
                        AttemptTime = x.AttemptTime,
                        IsSuccess = x.IsSuccess,
                        FailureReason = x.FailureReason ?? "",
                        IpAddress = x.IpAddress ?? ""
                    })
                    .ToList();

                return new PagedResult<LoginAttemptDto>
                {
                    Items = page.AsReadOnly(),
                    TotalCount = totalCount,
                    PageNumber = pagination.PageNumber,
                    PageSize = take
                };
            }
            catch (Exception)
            {
                // Return empty result on error instead of throwing
                return new PagedResult<LoginAttemptDto>
                {
                    Items = new List<LoginAttemptDto>().AsReadOnly(),
                    TotalCount = 0,
                    PageNumber = request.PageNumber,
                    PageSize = request.Length > 0 ? request.Length : 10
                };
            }
        }

        // Read-only behavior (keeps interface satisfied)
        public Task SaveAsync(LoginAttemptCommand command, string userUuid, string ip)
            => throw new NotSupportedException("Login attempt logs are read-only.");
        public Task DeleteAsync(string uuid, string userUuid, string ip)
            => throw new NotSupportedException("Login attempt logs are read-only.");
        public Task<bool> ToggleActiveAsync(string uuid, string userUuid, string ip)
            => throw new NotSupportedException("Login attempt logs are read-only.");
        public Task<LoginAttemptDto?> GetByUuidAsync(string uuid) => Task.FromResult<LoginAttemptDto?>(null);
        public Task<List<VerifyIndia.Application.DTO.DropDown.MasterDropDownDto>> GetDropdownAsync(Func<LoginAttemptCommand, string> displaySelector)
            => Task.FromResult(new List<VerifyIndia.Application.DTO.DropDown.MasterDropDownDto>());

        private static LoginAttemptDto? Deserialize(string? payload)
        {
            if (string.IsNullOrWhiteSpace(payload)) return null;
            try { return JsonSerializer.Deserialize<LoginAttemptDto>(payload); }
            catch { return null; }
        }

        private static List<LoginAttemptDto> ApplySort(List<LoginAttemptDto> rows, string? sortColumn, string? sortDirection)
        {
            var asc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);

            return (sortColumn ?? "").Trim().ToLowerInvariant() switch
            {
                "date" or "attempttime" => asc ? rows.OrderBy(x => x.AttemptTime).ToList()
                                               : rows.OrderByDescending(x => x.AttemptTime).ToList(),
                "user" or "username" => asc ? rows.OrderBy(x => x.UserName).ToList()
                                           : rows.OrderByDescending(x => x.UserName).ToList(),
                "status" or "issuccess" => asc ? rows.OrderBy(x => x.IsSuccess).ToList()
                                               : rows.OrderByDescending(x => x.IsSuccess).ToList(),
                "reason" or "failureReason" => asc ? rows.OrderBy(x => x.FailureReason).ToList()
                                                   : rows.OrderByDescending(x => x.FailureReason).ToList(),
                "ip" or "ipaddress" => asc ? rows.OrderBy(x => x.IpAddress).ToList()
                                          : rows.OrderByDescending(x => x.IpAddress).ToList(),
                _ => rows
            };
        }

        public Task SaveAsync(LoginAttemptCommand command, string userUuid, string ip, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}