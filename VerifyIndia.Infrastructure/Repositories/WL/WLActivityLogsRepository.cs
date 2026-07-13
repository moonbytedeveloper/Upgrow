using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities.WL.Master;
using VerifyIndia.Domain.IRepositories.WL;

namespace VerifyIndia.Infrastructure.Repositories.WL
{
    public class WLActivityLogsRepository : IWLActivityLogsRepository
    {
        private readonly AppDbContext _context;

        public WLActivityLogsRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add activity log to database
        /// </summary>
        public async Task AddAsync(WL_ActivityLogs log)
        {
            await _context.WL_ActivityLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get paged activity logs filtered by current tenant - deserializes from PayLoad
        /// </summary>
        public async Task<PagedResult<WL_ActivityLogs>> GetPagedAsync(
            string? menuName = null,
            string? search = null,
            PaginationParams? pagination = null)
        {
            pagination ??= new PaginationParams();

            var query = _context.WL_ActivityLogs
                .AsNoTracking()
                .AsQueryable()
                .Where(x => x.TenantId == _context.CurrentTenantId); // Tenant filter

            // Menu filter - search in JSON Payload
            if (!string.IsNullOrWhiteSpace(menuName))
            {
                query = query.Where(x =>
                    x.PayLoad != null &&  
                    x.PayLoad.Contains(menuName));
            }

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.PayLoad != null &&  
                    x.PayLoad.Contains(search));
            }

            // Total count BEFORE pagination (includes all filters)
            var totalCount = await query.CountAsync();

            // Server-side pagination
            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip(pagination.Skip)
                .Take(pagination.PageSize)
                .ToListAsync();

            // Deserialize only the paginated items if needed
            var deserializedItems = items
                .Select(x => DeserializeToActivityLog(x, menuName, search))
                .Where(x => x != null)
                .Cast<WL_ActivityLogs>()
                .ToList();

            return new PagedResult<WL_ActivityLogs>
            {
                Items = deserializedItems.AsReadOnly(),
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Get total count of activity logs for current tenant
        /// </summary>
        public async Task<int> GetTotalCountAsync(string? menuName = null)
        {
            var query = _context.WL_ActivityLogs
                .AsNoTracking()
                .Where(x => x.TenantId == _context.CurrentTenantId);
            return await query.CountAsync();
        }

        /// <summary>
        /// Get the last CurrentHash for tenant-specific hash chain integrity
        /// This maintains the hash chain for integrity verification per tenant
        /// </summary>
        public async Task<byte[]?> GetLastCurrentHashAsync()
        {
            return await _context.WL_ActivityLogs
                .AsNoTracking()
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

            var users = await _context.WL_MasterEmployee
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
        /// Deserialize PayLoad JSON to WL_ActivityLogs entity for display
        /// </summary>
        private WL_ActivityLogs? DeserializeToActivityLog(WL_ActivityLogs log, string? menuName, string? search)
        {
            if (string.IsNullOrWhiteSpace(log.PayLoad))
                return null;

            try
            {
                var dto = JsonSerializer.Deserialize<WLActivityLogDto>(log.PayLoad);
                if (dto == null)
                    return null;

                // Apply filters if provided
                if (!string.IsNullOrWhiteSpace(menuName))
                {
                    if ((dto.MenuName ?? "").ToLower() != menuName.ToLower())
                        return null;
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var s = search.ToLower();
                    var matches = (dto.ActivityType ?? "").ToLower().Contains(s) ||
                                  (dto.Description ?? "").ToLower().Contains(s) ||
                                  (dto.PageUrl ?? "").ToLower().Contains(s) ||
                                  (dto.IPAddress ?? "").ToLower().Contains(s) ||
                                  (dto.UserUUID ?? "").ToLower().Contains(s);

                    if (!matches)
                        return null;
                }

                // Return log with deserialized fields for display
                return new WL_ActivityLogs
                {
                    Id = log.Id,
                    TenantId = log.TenantId,
                    IsActive = log.IsActive,
                    CurrentHash = log.CurrentHash,
                    PreviousHash = log.PreviousHash,
                    DigitalSignature = log.DigitalSignature,
                    PayLoad = log.PayLoad,
                };
            }
            catch
            {
                return null;
            }
        }
    }
}