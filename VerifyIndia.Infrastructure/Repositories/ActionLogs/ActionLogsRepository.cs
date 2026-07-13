using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.ActionLogs;

namespace VerifyIndia.Infrastructure.Repositories.ActionLogs
{
    public class ActionLogsRepository : IActionLogsRepository
    {
        private readonly AppDbContext _context;

        public ActionLogsRepository(AppDbContext context)
        {
            _context = context;
        }

 
        public async Task<ActionLog?> GetByIdAsync(decimal logId)
        {
            return await _context.ActionLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == logId);
        }

 
        public async Task<PagedResult<ActionLog>> GetPagedByEntityAsync(
           string entityName,
           string entityUUID,
           bool includeChildren,
           IReadOnlyCollection<string> lineEntityNames,
           Expression<Func<ActionLog, bool>>? searchFilter,
           PaginationParams pagination,
           Func<IQueryable<ActionLog>, IOrderedQueryable<ActionLog>>? orderBy = null)
        {
             
            var query = _context.ActionLogs.AsNoTracking();

 
            var headerLogsQuery = query.Where(x =>
                x.Payload.Contains($"\"EntityName\":\"{entityName}\"") &&
                x.Payload.Contains($"\"EntityUUID\":\"{entityUUID}\""));

            IQueryable<ActionLog> filtered = headerLogsQuery;
 
            if (includeChildren)
            {
                var childLogsQuery = query.Where(x =>
                    x.Payload.Contains($"\"RootEntityUUID\":\"{entityUUID}\""));

 
                if (lineEntityNames.Count > 0)
                {
                    var lineEntityCondition = string.Join(" OR ",
                        lineEntityNames.Select(le => $"x.Payload.Contains(\"\\\"EntityName\\\":\\\"{le}\\\"\")"));

                    childLogsQuery = childLogsQuery.Where(x =>
                        lineEntityNames.Any(le =>
                            x.Payload.Contains($"\"EntityName\":\"{le}\"") ||
                            x.Payload.Contains($".{le}\"")));
                }

 
                filtered = filtered.Union(childLogsQuery);
            }

 
             if (searchFilter != null)
             {
                filtered = filtered.Where(searchFilter);
             }

            // Total count after applying all filters (used for DataTables recordsFiltered)
            var totalCount = await filtered.CountAsync();

            // Apply ordering and pagination to the filtered query (all executed at DB level)
            var paged = await (orderBy != null
                ? orderBy(filtered).Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync()
                : filtered.OrderByDescending(x => x.Id).Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync());


            return new PagedResult<ActionLog>
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

        public async Task AddAsync(ActionLog log)
        {
            await _context.ActionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]?> GetLastCurrentHashAsync()
        {
            return await _context.ActionLogs
                .OrderByDescending(x => x.Id)
                .Select(x => x.CurrentHash)
                .FirstOrDefaultAsync();
        }
    }
}