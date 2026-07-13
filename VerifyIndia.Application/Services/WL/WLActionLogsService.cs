using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using VerifyIndia.Application.DTO.ActionLogs;
using VerifyIndia.Application.IServices.ActionLogs;
using VerifyIndia.Application.IServices.WL.ActionLogs;
using VerifyIndia.Application.Utilities;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL;
using VerifyIndia.Domain.IRepositories.ActionLogs;

namespace VerifyIndia.Application.Services.ActionLogs
{
    public class WLActionLogsService : IWLActionLogsService
    {
        private readonly IWLActionLogsRepository _repository;
 

        public WLActionLogsService(IWLActionLogsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionLogDto?> GetByIdAsync(decimal logId)
        {
            var log = await _repository.GetByIdAsync(logId);
            if (log == null) return null;

            var payload = ParsePayload(log.Payload);
            var userUUID = GetValueFromPayload(payload, "UserUUID");

            //   OPTIMIZED: Only query user name for THIS user
            var users = await _repository.GetUserNamesByUuidsAsync(
                userUUID != null ? new[] { userUUID } : Array.Empty<string>()
            );

            return MapToDto(log, payload, users);
        }

        /// <summary>
        /// Gets paged action logs - optimized.
        /// </summary>
        public async Task<PagedResult<ActionLogDto>> GetPagedByEntityAsync(
            string entityName,
            string entityUUID,
            bool includeChildren,
            string? lineEntityNames,
            string? searchTerm,
            PaginationParams pagination)
        {
            var lineEntities = (lineEntityNames ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            //  OPTIMIZED: Build search filter efficiently
            Expression<Func<WL_ActionLogs, bool>>? searchFilter = null;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                // Search in payload JSON directly
                searchFilter = log =>
                    log.Payload != null && log.Payload.ToLower().Contains(searchTerm);
            }

            //  OPTIMIZED: Repository handles filtering in database
            var result = await _repository.GetPagedByEntityAsync(
                entityName,
                entityUUID,
                includeChildren,
                lineEntities,
                searchFilter,
                pagination);

            //   OPTIMIZED: Parse payloads only for current page
            var payloads = new Dictionary<decimal, Dictionary<string, object?>>();
            foreach (var log in result.Items)
            {
                payloads[log.Id] = ParsePayload(log.Payload);
            }

            //   OPTIMIZED: Batch user lookup - get all user IDs first, then query once
            var userIds = result.Items
                .Select(log => GetValueFromPayload(payloads[log.Id], "UserUUID"))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var users = userIds.Any()
                ? await _repository.GetUserNamesByUuidsAsync(userIds)
                : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            //   OPTIMIZED: Map DTOs using cached payloads
            var dtos = result.Items.Select(log => MapToDto(log, payloads[log.Id], users)).ToList();

            return new PagedResult<ActionLogDto>
            {
                Items = dtos,
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        /// <summary>
        /// Parse JSON payload safely with error handling.
        /// </summary>
        private Dictionary<string, object?> ParsePayload(string? payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
                return new Dictionary<string, object?>();

            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, object?>>(payload) ??
                    new Dictionary<string, object?>();
            }
            catch
            {
                return new Dictionary<string, object?>();
            }
        }

        /// <summary>
        /// Get value from payload dictionary safely.
        /// </summary>
        private string? GetValueFromPayload(Dictionary<string, object?> payload, string key)
        {
            if (payload.TryGetValue(key, out var value))
            {
                return value?.ToString();
            }
            return null;
        }

        /// <summary>
        /// Map ActionLog entity to ActionLogDto.
        /// </summary>
        private ActionLogDto MapToDto(
            WL_ActionLogs log,
            Dictionary<string, object?> payload,
            Dictionary<string, string> userNameMap)
        {
            var userUUID = GetValueFromPayload(payload, "UserUUID");

            return new ActionLogDto
            {
                Id = log.Id,
                EntityName = GetValueFromPayload(payload, "EntityName"),
                EntityUUID = GetValueFromPayload(payload, "EntityUUID"),
                RootEntityName = GetValueFromPayload(payload, "RootEntityName"),
                RootEntityUUID = GetValueFromPayload(payload, "RootEntityUUID"),
                ActionType = GetValueFromPayload(payload, "ActionType"),
                OldValues = GetValueFromPayload(payload, "OldValues"),
                NewValues = GetValueFromPayload(payload, "NewValues"),
                UserUUID = userUUID,
                IPAddress = GetValueFromPayload(payload, "IPAddress"),
                CreatedAt = ParseDateTime(GetValueFromPayload(payload, "CreatedAt")),
                UserName = userNameMap.TryGetValue(userUUID ?? string.Empty, out var name)
                    ? name
                    : (userUUID ?? "-")
            };
        }

        /// <summary>
        /// Parse DateTime safely.
        /// </summary>
        private DateTimeOffset? ParseDateTime(string? dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr))
                return null;

            try
            {
                return DateTimeOffset.Parse(dateStr);
            }
            catch
            {
                return null;
            }
        }

    }
}