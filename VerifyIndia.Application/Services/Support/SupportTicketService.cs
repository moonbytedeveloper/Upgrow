using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Support;
using VerifyIndia.Application.DTO.Support;
using VerifyIndia.Application.IServices.Support;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Support;

namespace VerifyIndia.Application.Services.Support
{
    public class SupportTicketService : ISupportTicketService
    {
        private readonly ISupportTicketRepository _repository;


        public SupportTicketService(ISupportTicketRepository repository)
        {
            _repository = repository;
        }
        private List<ActiveSupportTicketDto> MapTickets(
           List<(Support_TicketHeader,Support_TicketLine)> data)
        {
            return data
    .OrderByDescending(t => t.Item1.UpdatedOn)
    .Select(t => new ActiveSupportTicketDto
    {
        UUID = t.Item1.UUID ?? string.Empty,
        TicketNumber = t.Item1.TicketNumber ?? string.Empty,
        Subject = t.Item1.Subject ?? string.Empty,
        Message = !string.IsNullOrWhiteSpace(t.Item2?.Message)
            ? t.Item2.Message
            : "No message",
        Otp = t.Item1.Otp ?? string.Empty,
        IsActive = t.Item1.IsActive,
        CreatedOn = t.Item1.CreatedAt,
        UpdatedOn = t.Item1.UpdatedOn,
    })
    .ToList();
        }
        /// <summary>
        /// Retrieves all support tickets for a user (active and closed) with messages from lines.
        /// </summary>
        public async Task<List<ActiveSupportTicketDto>> GetAllAsync(string userUuid)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
                throw new ArgumentException("User UUID cannot be null or empty.", nameof(userUuid));

            var tickets = await _repository.GetAllTicketHeadersWithLinesAsync(userUuid);

            return MapTickets(tickets);
        }

        /// <summary>
        /// Retrieves only active support tickets for a user with messages from lines.
        /// </summary>
        public async Task<List<ActiveSupportTicketDto>> GetActiveAsync(string userUuid)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
                throw new ArgumentException("User UUID cannot be null or empty.", nameof(userUuid));

            var tickets = await _repository.GetActiveTicketHeadersWithLinesAsync(userUuid);

            return MapTickets(tickets);
        }

        /// <summary>
        /// Retrieves only closed support tickets for a user with messages from lines.
        /// </summary>
        public async Task<List<ActiveSupportTicketDto>> GetClosedAsync(string userUuid)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
                throw new ArgumentException("User UUID cannot be null or empty.", nameof(userUuid));

            var tickets = await _repository.GetClosedTicketHeadersWithLinesAsync(userUuid);

            return MapTickets(tickets);
        }

        /// <summary>
        /// Get tickets by status (open, closed, or all)
        /// </summary>
        public async Task<List<ActiveSupportTicketDto>>   GetByStatusAsync(string userUuid, string? status = null)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
                throw new ArgumentException("User UUID cannot be null or empty.", nameof(userUuid));

            var result = status?.ToLowerInvariant().Trim() switch
            {
                "open" => await GetActiveAsync(userUuid),
                "closed" => await GetClosedAsync(userUuid),
                _ => await GetAllAsync(userUuid)  // null, empty, or any other value returns all
            };

            return result;
        }

        public async Task<PagedResult<ActiveSupportTicketDto>> GetPagedByStatusAsync(
             string userUuid,
             string? status = null,
             PaginationParams? pagination = null,
             string? search = null,
             string? sortColumn = null,
             string? sortOrder = null)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
                throw new ArgumentException("User UUID cannot be null or empty.", nameof(userUuid));

            pagination ??= new PaginationParams();

            bool? isActive = status?.ToLowerInvariant().Trim() switch
            {
                "open" => true,
                "closed" => false,
                _ => null
            };

            var tickets = await _repository.GetPagedTicketHeadersByUserAsync(
                userUuid,
                isActive,
                pagination,
                search,
                sortColumn,
                sortOrder);

            return new PagedResult<ActiveSupportTicketDto>
            {
                Items = tickets.Items.Select(t => new ActiveSupportTicketDto
                {
                    UUID = t.UUID,
                    TicketNumber = t.TicketNumber,
                    Subject = t.Subject,
                    Message = string.IsNullOrWhiteSpace(t.Message) ? "No message" : t.Message,
                    Otp = t.Otp,
                    IsActive = t.IsActive,
                    CreatedOn = t.CreatedAt,
                    AssignedTo = t.AssigneeUUID,
                    UpdatedOn = t.UpdatedOn,
                    Category = t.TicketCategoryUUID

                }).ToList().AsReadOnly(),
                TotalCount = tickets.TotalCount,
                PageNumber = tickets.PageNumber,
                PageSize = tickets.PageSize
            };
        }
    }
}