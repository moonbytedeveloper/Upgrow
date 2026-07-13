using System.Linq.Expressions;
using System.Text.Json;
using VerifyIndia.Application.DTO;
using VerifyIndia.Application.IServices;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services
{
    public class ActivityLogsService : IActivityLogsService
    {
        private readonly IActivityLogsRepository _repository;

        public ActivityLogsService(IActivityLogsRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ActivityLogDto>> GetPagedAsync(
            string? menuName = null,
            string? search = null,
            PaginationParams? pagination = null)
        {
            pagination ??= new PaginationParams();

            // Build search expression
            Expression<Func<ActivityLogs, bool>>? searchFilter = null;

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                searchFilter = x =>
                    x.Payload != null &&
                    x.Payload.ToLower().Contains(search);
            }

            // Repository handles pagination + filtering
            var result = await _repository.GetPagedAsync(
                menuName,
                searchFilter,
                pagination);

            // Parse payloads ONLY for current page
            var payloads = new Dictionary<decimal, Dictionary<string, object?>>();

            foreach (var log in result.Items)
            {
                payloads[log.Id] = ParsePayload(log.Payload);
            }

            // Get distinct user UUIDs
            var userUuids = result.Items
                .Select(x => GetValue(payloads[x.Id], "UserUUID"))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Batch fetch user names
            var userNames = userUuids.Any()
                ? await _repository.GetUserNamesByUuidsAsync(userUuids)
                : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Map DTOs
            var dtos = result.Items
                .Select(x => MapToDto(x, payloads[x.Id], userNames))
                .ToList();

            return new PagedResult<ActivityLogDto>
            {
                Items = dtos,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }

        public async Task<int> GetTotalCountAsync(string? menuName = null)
        {
            return await _repository.GetTotalCountAsync(menuName);
        }

        private Dictionary<string, object?> ParsePayload(string? payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
                return new Dictionary<string, object?>();

            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, object?>>(payload)
                    ?? new Dictionary<string, object?>();
            }
            catch
            {
                return new Dictionary<string, object?>();
            }
        }

        private string? GetValue(Dictionary<string, object?> payload, string key)
        {
            return payload.TryGetValue(key, out var value)
                ? value?.ToString()
                : null;
        }

        private DateTimeOffset ParseDateTime(string? value)
        {
            if (DateTimeOffset.TryParse(value, out var result))
                return result;

            return DateTimeOffset.MinValue;
        }

        private ActivityLogDto MapToDto(
            ActivityLogs log,
            Dictionary<string, object?> payload,
            Dictionary<string, string> userNames)
        {
            var userUUID = GetValue(payload, "UserUUID");

            return new ActivityLogDto
            {
                UserUUID = userUUID ?? string.Empty,

                EmployeeName =
                    !string.IsNullOrWhiteSpace(userUUID) &&
                    userNames.TryGetValue(userUUID, out var name)
                        ? name
                        : (userUUID ?? string.Empty),

                ActivityType = GetValue(payload, "ActivityType") ?? string.Empty,

                Description = GetValue(payload, "Description") ?? string.Empty,

                MenuName = GetValue(payload, "MenuName") ?? string.Empty,

                PageUrl = GetValue(payload, "PageUrl") ?? string.Empty,

                IPAddress = GetValue(payload, "IPAddress") ?? string.Empty,

                CreatedAt = ParseDateTime(GetValue(payload, "CreatedAt"))
            };
        }
    }
}