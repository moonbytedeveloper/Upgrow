using System.Linq.Expressions;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories.ActionLogs
{
    public interface IActionLogsRepository
    {
 

        Task<ActionLog?> GetByIdAsync(decimal logId);

        Task<PagedResult<ActionLog>> GetPagedByEntityAsync(
            string entityName,
            string entityUUID,
            bool includeChildren,
            IReadOnlyCollection<string> lineEntityNames,
            Expression<Func<ActionLog, bool>>? searchFilter,
            PaginationParams pagination,
            Func<IQueryable<ActionLog>, IOrderedQueryable<ActionLog>>? orderBy = null);

        Task<Dictionary<string, string>> GetUserNamesByUuidsAsync(
            IReadOnlyCollection<string> userUuids);

        Task AddAsync(ActionLog log);

        Task<byte[]?> GetLastCurrentHashAsync();

    }
}