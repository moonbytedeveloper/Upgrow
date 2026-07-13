using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO;
using Upgrow.Domain.Common;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.IServices
{
    public interface IActivityLogsService
    {
        /// <summary>
        /// Get paged activity logs with employee names
        /// </summary>
        Task<PagedResult<ActivityLogDto>> GetPagedAsync(
            string? menuName = null,
            string? search = null,
            PaginationParams? pagination = null);

        /// <summary>
        /// Get total count of activity logs
        /// </summary>
        Task<int> GetTotalCountAsync(string? menuName = null);
    }
}
