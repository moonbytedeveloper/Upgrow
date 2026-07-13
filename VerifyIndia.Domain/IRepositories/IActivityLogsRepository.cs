using System.Linq.Expressions;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
namespace Upgrow.Domain.IRepositories
{
    public interface IActivityLogsRepository
    {
        /// <summary>
        /// Get paged activity logs
        /// </summary>
        Task<PagedResult<ActivityLogs>> GetPagedAsync(
         string? menuName = null,
         Expression<Func<ActivityLogs, bool>>? searchFilter = null,
         PaginationParams? pagination = null);

        /// <summary>
        /// Get total count of activity logs
        /// </summary>
        Task<int> GetTotalCountAsync(string? menuName = null);

        Task<byte[]?> GetLastCurrentHashAsync();
        Task AddAsync(ActivityLogs log);
        Task<Dictionary<string, string>> GetUserNamesByUuidsAsync(IReadOnlyCollection<string> userUuids);
    }
}
