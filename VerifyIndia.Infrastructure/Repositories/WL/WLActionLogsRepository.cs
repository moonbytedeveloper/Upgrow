using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.WL;
using VerifyIndia.Domain.IRepositories.ActionLogs;
using VerifyIndia.Infrastructure.Extensions;

namespace VerifyIndia.Infrastructure.Repositories.ActionLogs
{
    public class WLActionLogsRepository : IWLActionLogsRepository
    {
        private readonly AppDbContext _context;

        public WLActionLogsRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<WL_ActionLogs?> GetByIdAsync(decimal logId)
        {
            return await _context.WL_ActionLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == logId && x.TenantId == _context.CurrentTenantId);
        }


        public async Task<PagedResult<WL_ActionLogs>> GetPagedByEntityAsync(
           string entityName,
           string entityUUID,
           bool includeChildren,
           IReadOnlyCollection<string> lineEntityNames,
           Expression<Func<WL_ActionLogs, bool>>? searchFilter,
           PaginationParams pagination,
           Func<IQueryable<WL_ActionLogs>, IOrderedQueryable<WL_ActionLogs>>? orderBy = null)
        {
            var query = _context.WL_ActionLogs
            .AsNoTracking()
            .Where(x => x.TenantId == _context.CurrentTenantId);

            // Base: logs whose payload contains the specified entity & uuid
            var headerLogsQuery = query.Where(x =>
                x.Payload != null &&
                x.Payload.Contains($"\"EntityName\":\"{entityName}\"") &&
                x.Payload.Contains($"\"EntityUUID\":\"{entityUUID}\""));

            IQueryable<WL_ActionLogs> filtered = headerLogsQuery;

            if (includeChildren)
            {
                var childLogsQuery = query.Where(x =>
                    x.Payload != null &&
                    x.Payload.Contains($"\"RootEntityUUID\":\"{entityUUID}\""));

                if (lineEntityNames.Count > 0)
                {
                    childLogsQuery = childLogsQuery.Where(x =>
                        lineEntityNames.Any(le =>
                            (x.Payload ?? string.Empty).Contains($"\"EntityName\":\"{le}\"") ||
                            (x.Payload ?? string.Empty).Contains($".{le}\"")));
                }

                filtered = filtered.Union(childLogsQuery);
            }

            // Apply search filter at DB level (if provided) BEFORE counting & pagination
            if (searchFilter != null)
            {
                filtered = filtered.Where(searchFilter);
            }

            // Total count after applying all filters (this is what DataTables expects for recordsFiltered)
            var totalCount = await filtered.CountAsync();

            // Pagination and ordering applied to filtered query
            var paged = await (orderBy != null
                ? orderBy(filtered).Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync()
                : filtered.OrderByDescending(x => x.Id).Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync());

            return new PagedResult<WL_ActionLogs>
            {
                Items = paged,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }


        public async Task<Dictionary<string, string>> GetUserNamesByUuidsAsync(
            IReadOnlyCollection<string> userUuids)
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

        public async Task AddAsync(WL_ActionLogs log)
        {
            await _context.WL_ActionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]?> GetLastCurrentHashAsync()
        {
            return await _context.WL_ActionLogs
                .OrderByDescending(x => x.Id)
                .Select(x => x.CurrentHash)
                .FirstOrDefaultAsync();
        }
    }
}