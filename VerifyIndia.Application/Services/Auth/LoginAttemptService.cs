using System.Text.Json;
using Upgrow.Application.Commands.Auth;
using Upgrow.Application.DTO.Auth;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Auth;
using Upgrow.Domain.Common;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Auth
{
    public sealed class LoginAttemptService : ILoginAttemptService
    {
        private readonly ILoginAttemptRepository _repo;

        public LoginAttemptService(ILoginAttemptRepository repo)
        {
            _repo = repo;
        }

        // public LoginAttemptService(IMasterRepository<LoginAttempts> repository, IMapper mapper)
        //     : base(repository, mapper)
        // {
        // }

        // No duplicate concept for logs
        // protected override Task<bool> IsDuplicateAsync(LoginAttemptCommand command)
        // {
        //     return Task.FromResult(false);
        // }

        // Global search - DataTable search box
        // protected override Expression<Func<LoginAttempts, bool>>? BuildSearchFilter(string searchTerm)
        // {
        //     if (string.IsNullOrWhiteSpace(searchTerm))
        //         return null;

        //     searchTerm = searchTerm.ToLower();

        //     return x =>
        //         (x.UserName != null && x.UserName.ToLower().Contains(searchTerm)) ||
        //         (x.FailureReason != null && x.FailureReason.ToLower().Contains(searchTerm)) ||
        //         // Search by status - matches "success" or "failed" (case-insensitive)
        //         (searchTerm.Contains("success") && x.IsSuccess) ||
        //         (searchTerm.Contains("failed") && !x.IsSuccess) ||
        //     (x.IpAddress != null && x.IpAddress.ToLower().Contains(searchTerm)); /*||
        //         // Search by date - supports multiple formats
        //         x.AttemptTime.ToString("dd-MMM-yyyy").ToLower().Contains(searchTerm) ||  // "11-Apr-2026"
        //         x.AttemptTime.ToString("dd-MMM-yyyy hh:mm tt").ToLower().Contains(searchTerm) ||  // "11-Apr-2026 10:38 AM"
        //         x.AttemptTime.ToString("dd-MMM").ToLower().Contains(searchTerm) ||  // "11-Apr"
        //         x.AttemptTime.ToString("hh:mm tt").ToLower().Contains(searchTerm)*/  // "10:38 AM"
        // }

        // // Sorting for DataTable columns
        // protected override Func<IQueryable<LoginAttempts>, IOrderedQueryable<LoginAttempts>>? BuildSortExpression(
        //     string? sortColumn,
        //     string? sortDirection)
        // {
        //     var isAsc = sortDirection?.ToLower() == "asc";

        //     return sortColumn?.ToLower() switch
        //     {
        //         "user" or "username" => q => isAsc
        //             ? q.OrderBy(x => x.UserName)
        //             : q.OrderByDescending(x => x.UserName),

        //         "date" or "attempttime" => q => isAsc
        //             ? q.OrderBy(x => x.AttemptTime)
        //             : q.OrderByDescending(x => x.AttemptTime),

        //         "status" => q => isAsc
        //             ? q.OrderBy(x => x.IsSuccess)
        //             : q.OrderByDescending(x => x.IsSuccess),

        //         "reason" or "failurereason" => q => isAsc
        //             ? q.OrderBy(x => x.FailureReason)
        //             : q.OrderByDescending(x => x.FailureReason),

        //         "ip" or "ipaddress" => q => isAsc
        //             ? q.OrderBy(x => x.IpAddress)
        //             : q.OrderByDescending(x => x.IpAddress),

        //         _ => q => isAsc
        //             ? q.OrderBy(x => x.Id)
        //             : q.OrderByDescending(x => x.Id)
        //     };
        // }

        public Task<PagedResult<LoginAttemptDto>> GetPagedAsync(DataTableRequest request)
            => GetPagedByUserAsync(request, userUuid: null);

        public async Task<PagedResult<LoginAttemptDto>> GetPagedByUserAsync(DataTableRequest request, string? userUuid)
        {
            // Pull a reasonable window then filter/sort/page in memory.
            // Payload storage prevents easy DB-side filtering.
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
                        (!string.IsNullOrWhiteSpace(x.UserName) && x.UserName.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(x.FailureReason) && x.FailureReason.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(x.IpAddress) && x.IpAddress.Contains(s, StringComparison.OrdinalIgnoreCase)))
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
                Items = page,
                TotalCount = totalCount,
                PageNumber = (request.Start / take) + 1,
                PageSize = take
            };
        }

        // Read-only behavior (keeps interface satisfied)
        public Task SaveAsync(LoginAttemptCommand command, string userUuid, string ip, bool saveChanges = true)
            => throw new NotSupportedException("Login attempt logs are read-only.");
        public Task DeleteAsync(string uuid, string userUuid, string ip)
            => throw new NotSupportedException("Login attempt logs are read-only.");
        public Task<bool> ToggleActiveAsync(string uuid, string userUuid, string ip)
            => throw new NotSupportedException("Login attempt logs are read-only.");
        public Task<LoginAttemptDto?> GetByUuidAsync(string uuid) => Task.FromResult<LoginAttemptDto?>(null);
        public Task<List<Upgrow.Application.DTO.DropDown.MasterDropDownDto>> GetDropdownAsync(Func<LoginAttemptCommand, string> displaySelector)
            => Task.FromResult(new List<Upgrow.Application.DTO.DropDown.MasterDropDownDto>());

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

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}