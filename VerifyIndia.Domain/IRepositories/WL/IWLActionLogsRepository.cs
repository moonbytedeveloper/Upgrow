using System.Linq.Expressions;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities.WL;

namespace Upgrow.Domain.IRepositories.ActionLogs
{
    public interface IWLActionLogsRepository
    {
        Task<WL_ActionLogs?> GetByIdAsync(decimal logId);

        Task<PagedResult<WL_ActionLogs>> GetPagedByEntityAsync(
            string entityName,
            string entityUUID,
            bool includeChildren,
            IReadOnlyCollection<string> lineEntityNames,
            Expression<Func<WL_ActionLogs, bool>>? searchFilter,
            PaginationParams pagination,
            Func<IQueryable<WL_ActionLogs>, IOrderedQueryable<WL_ActionLogs>>? orderBy = null);

        Task<Dictionary<string, string>> GetUserNamesByUuidsAsync(
            IReadOnlyCollection<string> userUuids);

        Task AddAsync(WL_ActionLogs log);

        Task<byte[]?> GetLastCurrentHashAsync();

    }
}