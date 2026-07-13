using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.DTO;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Infrastructure.Repositories
{
    public class ActivityLogsRepository : IActivityLogsRepository
    {
        private readonly AppDbContext _context;

        public ActivityLogsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ActivityLogs log)
        {
            await _context.ActivityLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ActivityLogs>> GetPagedAsync(
        string? menuName = null,
        Expression<Func<ActivityLogs, bool>>? searchFilter = null,
        PaginationParams? pagination = null)
        {
            pagination ??= new PaginationParams();

            var query = _context.ActivityLogs
                .AsNoTracking()
                .AsQueryable();

            // Menu filter
            if (!string.IsNullOrWhiteSpace(menuName))
            {
                query = query.Where(x =>
                    x.Payload != null &&
                    (
                        x.Payload.Contains(menuName)
                    ));
            }

            // Search filter
            if (searchFilter != null)
            {
                query = query.Where(searchFilter);
            }

            // Total count BEFORE pagination
            var totalCount = await query.CountAsync();

            // PURE SERVER SIDE PAGINATION
            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip(pagination.Skip)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagedResult<ActivityLogs>
            {
                Items = items,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<int> GetTotalCountAsync(string? menuName = null)
        {
            var allActivityLogs = await _context.ActivityLogs
                .AsNoTracking()
                .ToListAsync();

            var query = allActivityLogs.Where(x => true).ToList();

            if (!string.IsNullOrWhiteSpace(menuName))
            {
                query = query
                    .Where(x =>
                        (DeserializePayload(x.Payload, "MenuName")?.Contains(menuName) ?? false) ||
                        (DeserializePayload(x.Payload, "PageUrl") == menuName))
                    .ToList();
            }

            return query.Count;
        }

        public async Task<byte[]?> GetLastCurrentHashAsync()
        {
            return await _context.ActivityLogs
                .OrderByDescending(x => x.Id)
                .Select(x => x.CurrentHash)
                .FirstOrDefaultAsync();
        }

        public async Task<Dictionary<string, string>> GetUserNamesByUuidsAsync(IReadOnlyCollection<string> userUuids)
        {
            if (userUuids.Count == 0)
            {
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            var users = await _context.Master_Employee
                .AsNoTracking()
                .Where(x => x.UUID != null && userUuids.Contains(x.UUID))
                .Select(x => new
                {
                    x.UUID,
                    FullName = (x.FirstName ?? "") + " " + (x.LastName ?? "")
                })
                .ToListAsync();

            return users
                .Where(x => !string.IsNullOrWhiteSpace(x.UUID))
                .ToDictionary(x => x.UUID!, x => x.FullName.Trim(), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Single deserialization method to extract any field from the Payload JSON
        /// </summary>
        private static string? DeserializePayload(string? payload, string fieldName)
        {
            if (string.IsNullOrEmpty(payload)) return null;
            try
            {
                var data = JsonSerializer.Deserialize<Dictionary<string, object?>>(payload ?? "{}");
                return data?.TryGetValue(fieldName, out var value) ?? false ? value?.ToString() : null;
            }
            catch
            {
                return null;
            }
        }

 
    }
}