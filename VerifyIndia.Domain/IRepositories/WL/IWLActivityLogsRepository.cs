using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities.WL.Master;

namespace VerifyIndia.Domain.IRepositories.WL
{
    public interface IWLActivityLogsRepository
    {
        /// <summary>
        /// Get paged activity logs with employee name join
        /// </summary>
        Task<PagedResult<WL_ActivityLogs>> GetPagedAsync(
            string? menuName = null,
            string? search = null,
            PaginationParams? pagination = null);

        /// <summary>
        /// Get total count of activity logs
        /// </summary>
        Task<int> GetTotalCountAsync(string? menuName = null);

        /// <summary>
        /// Get the last CurrentHash (to use as PreviousHash for next entry)
        /// This maintains the hash chain for integrity verification
        /// </summary>
        /// <summary>
        /// Get the last CurrentHash for tenant-specific hash chain integrity
        /// This maintains the hash chain for integrity verification per tenant
        /// </summary>
        Task<byte[]?> GetLastCurrentHashAsync();

        /// <summary>
        /// Add activity log to database
        /// </summary>
        Task AddAsync(WL_ActivityLogs log);
        Task<Dictionary<string, string>> GetUserNamesByUuidsAsync(IReadOnlyCollection<string> userUuids);
    }
}
