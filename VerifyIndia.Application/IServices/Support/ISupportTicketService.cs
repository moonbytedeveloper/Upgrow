using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Support;
using Upgrow.Application.DTO.Support;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Common;

namespace Upgrow.Application.IServices.Support
{
    public interface ISupportTicketService 
    {
        Task<List<ActiveSupportTicketDto>> GetAllAsync(string userUuid);
        Task<List<ActiveSupportTicketDto>> GetActiveAsync(string userUuid);
        Task<List<ActiveSupportTicketDto>> GetClosedAsync(string userUuid);
        /// <summary>
        /// Get support tickets filtered by status
        /// </summary>
        /// <param name="userUuid">User/Customer UUID</param>
        /// <param name="status">Filter: "open", "closed", or null/empty for all</param>
        Task<List<ActiveSupportTicketDto>> GetByStatusAsync(string userUuid, string? status = null);

        Task<PagedResult<ActiveSupportTicketDto>> GetPagedByStatusAsync(
            string userUuid,
            string? status = null,
            PaginationParams? pagination = null,
            string? search = null,
            string? sortColumn = null,
            string? sortOrder = null);

    }
}

