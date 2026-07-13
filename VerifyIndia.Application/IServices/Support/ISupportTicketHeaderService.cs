using Upgrow.Application.Commands.Support;
using Upgrow.Application.DTO.Support;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Common;

namespace Upgrow.Application.IServices.Support
{
    public interface ISupportTicketHeaderService : IMasterService<SupportTicketDto, SupportTicketHeaderCommand>
    {
        Task<PagedResult<SupportTicketDto>> GetPagedAsync(
           DataTableRequest request,
           string? userType = null,
           string? categoryUuid = null,
           string? assigneeUuid = null, bool? isActive = null, string? currentUserUuid = null);

        Task<List<SupportTicketAssignableEmployeeDto>> GetAssignableEmployeesAsync(string ticketCategoryUuid);
        Task AssignTicketAsync(string ticketUuid, string assigneeUuid, string userUuid, string ip);

        Task<List<TicketConversationDto>> GetTicketConversationAsync(string ticketUuid);

        Task AddReplyAsync(
    string ticketUuid,
    string message,
    List<string> attachments,
    string userUUID,
    string ip);

        Task SaveAsync(
    SupportTicketHeaderCommand header,
    string message,
    List<string>? attachments,
    string userUuid,
    string ip);

        //Task<string> RegenerateOtpAsync(string ticketUuid);
    }
}