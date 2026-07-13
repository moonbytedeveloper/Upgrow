using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories.Support
{
    public interface ISupportTicketRepository 
    {
        Task<List<(Support_TicketHeader, Support_TicketLine)>> GetTicketsByUserAsync(string userUuid, bool? isActive);
        Task<List<Support_TicketHeader>> GetAllTicketHeadersAsync(string userUuid);
        Task<List<Support_TicketLine>> GetTicketLinesByHeaderAsync(string headerUuid);
        Task<List<(Support_TicketHeader, Support_TicketLine)>> GetAllTicketHeadersWithLinesAsync(string userUuid);
        Task<List<(Support_TicketHeader, Support_TicketLine)>> GetActiveTicketHeadersWithLinesAsync(string userUuid);
        Task<List<(Support_TicketHeader, Support_TicketLine)>> GetClosedTicketHeadersWithLinesAsync(string userUuid);

        Task<PagedResult<Support_TicketHeader>> GetPagedTicketHeadersByUserAsync(
            string userUuid,
            bool? isActive,
            PaginationParams pagination,
            string? search = null,
            string? sortColumn = null,
            string? sortOrder = null);
    }
}
