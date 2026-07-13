using Upgrow.Application.DTO.ActionLogs;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices.WL.ActionLogs
{
    public interface IWLActionLogsService
    {
        /// <summary>
        /// Gets a single action log by ID for detail view.
        /// Use this for fetching log details when user clicks "View" button.
        /// </summary>
        Task<ActionLogDto?> GetByIdAsync(decimal logId);

        /// <summary>
        /// Gets paged action logs for a given entity with search, filtering, and pagination.
        /// This is the PRIMARY method for retrieving logs - handles all server-side pagination.
        /// </summary>
        Task<PagedResult<ActionLogDto>> GetPagedByEntityAsync(
           string entityName,
           string entityUUID,
           bool includeChildren,
           string? lineEntityNames,
           string? searchTerm,
           PaginationParams pagination);


    }
}