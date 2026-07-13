using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Support;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.Support;

namespace Upgrow.Infrastructure.Repositories.Support
{
    public class SupportTicketRepository : ISupportTicketRepository
    {
        private readonly AppDbContext _context;

        public SupportTicketRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves support tickets for a specific user with optional active status filtering.
        /// Performs a left outer join between Support_TicketHeader and Support_TicketLine.
        /// </summary>
        public async Task<List<(Support_TicketHeader, Support_TicketLine)>> GetTicketsByUserAsync(string userUuid, bool? isActive)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
            {
                throw new ArgumentException("User UUID cannot be null or empty.", nameof(userUuid));
            }

            var query = from header in _context.Support_TicketHeader.AsNoTracking()
                        where header.UserUUID == userUuid
                        join line in _context.Support_TicketLine.AsNoTracking()
                            on new { header.UUID, userUuid } equals new { UUID = line.HeaderUUID, userUuid = line.UserUUID }
                            into lineGroup
                        from line in lineGroup.DefaultIfEmpty()
                        select new
                        {
                            header,
                            line
                        };

            if (isActive.HasValue)
            {
                query = query.Where(x => x.header.IsActive == isActive.Value);
            }

            var results = await query.ToListAsync();
            return results.Select(x => (x.header, x.line)).ToList();
        }

        /// <summary>
        /// Retrieves all support ticket headers for a specific user.
        /// </summary>
        public async Task<List<Support_TicketHeader>> GetAllTicketHeadersAsync(string userUuid)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
            {
                throw new ArgumentException("User UUID cannot be null or empty.", nameof(userUuid));
            }

            return await _context.Support_TicketHeader
                .AsNoTracking()
                .Where(h => h.UserUUID == userUuid)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all support ticket lines for a specific ticket header.
        /// </summary>
        public async Task<List<Support_TicketLine>> GetTicketLinesByHeaderAsync(string headerUuid)
        {
            if (string.IsNullOrWhiteSpace(headerUuid))
            {
                throw new ArgumentException("Header UUID cannot be null or empty.", nameof(headerUuid));
            }

            return await _context.Support_TicketLine
                .AsNoTracking()
                .Where(l => l.HeaderUUID == headerUuid)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a ticket header with all its associated lines.
        /// </summary>
        public async Task<(Support_TicketHeader, List<Support_TicketLine>)> GetTicketWithLinesAsync(string headerUuid)
        {
            if (string.IsNullOrWhiteSpace(headerUuid))
            {
                throw new ArgumentException("Header UUID cannot be null or empty.", nameof(headerUuid));
            }

            var header = await _context.Support_TicketHeader
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.UUID == headerUuid);

            if (header == null)
            {
                return (default, new List<Support_TicketLine>());
            }

            var lines = await _context.Support_TicketLine
                .AsNoTracking()
                .Where(l => l.HeaderUUID == headerUuid)
                .ToListAsync();

            return (header, lines);
        }

        /// <summary>
        /// Retrieves all support ticket headers with their latest message line for a specific user.
        /// Fetches headers first, then gets the latest line for each header.
        /// </summary>
        public async Task<List<(Support_TicketHeader, Support_TicketLine)>> GetAllTicketHeadersWithLinesAsync(string userUuid)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
            {
                throw new ArgumentException("User UUID cannot be null or empty.", nameof(userUuid));
            }

            // Get all headers for the user
            var headers = await _context.Support_TicketHeader
                .AsNoTracking()
                .Where(h => h.UserUUID == userUuid)
                .ToListAsync();

            if (!headers.Any())
                return new List<(Support_TicketHeader, Support_TicketLine)>();

            var headerUuids = headers.Select(h => h.UUID).ToList();

            // Get the latest line for each header
            var lines = await _context.Support_TicketLine
            .AsNoTracking()
            .Where(l => headerUuids.Contains(l.HeaderUUID))
            .GroupBy(l => l.HeaderUUID)
            .Select(g => g.OrderBy(l => l.CreatedAt).FirstOrDefault())
            .ToListAsync();

            var result = new List<(Support_TicketHeader, Support_TicketLine)>();
            foreach (var header in headers)
            {
                var line = lines.FirstOrDefault(l => l?.HeaderUUID == header.UUID);
                result.Add((header, line));
            }

            return result;
        }

        /// <summary>
        /// Retrieves active support ticket headers with their latest message line for a specific user.
        /// </summary>
        public async Task<List<(Support_TicketHeader, Support_TicketLine)>> GetActiveTicketHeadersWithLinesAsync(string userUuid)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
            {
                throw new ArgumentException("User UUID cannot be null or empty.", nameof(userUuid));
            }

            // Get all active headers for the user
            var headers = await _context.Support_TicketHeader
                .AsNoTracking()
                .Where(h => h.UserUUID == userUuid && h.IsActive)
                .ToListAsync();

            if (!headers.Any())
                return new List<(Support_TicketHeader, Support_TicketLine)>();

            var headerUuids = headers.Select(h => h.UUID).ToList();

            // Get the latest line for each header
            var lines = await _context.Support_TicketLine
             .AsNoTracking()
             .Where(l => headerUuids.Contains(l.HeaderUUID))
             .GroupBy(l => l.HeaderUUID)
             .Select(g => g.OrderBy(l => l.CreatedAt).FirstOrDefault())
             .ToListAsync();

            var result = new List<(Support_TicketHeader, Support_TicketLine)>();
            foreach (var header in headers)
            {
                var line = lines.FirstOrDefault(l => l?.HeaderUUID == header.UUID);
                result.Add((header, line));
            }

            return result;
        }

        /// <summary>
        /// Retrieves closed support ticket headers with their latest message line for a specific user.
        /// </summary>
        public async Task<List<(Support_TicketHeader, Support_TicketLine)>> GetClosedTicketHeadersWithLinesAsync(string userUuid)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
            {
                throw new ArgumentException("User UUID cannot be null or empty.", nameof(userUuid));
            }

            // Get all closed headers for the user
            var headers = await _context.Support_TicketHeader
                .AsNoTracking()
                .Where(h => h.UserUUID == userUuid && !h.IsActive)
                .ToListAsync();

            if (!headers.Any())
                return new List<(Support_TicketHeader, Support_TicketLine)>();

            var headerUuids = headers.Select(h => h.UUID).ToList();

            // Get the latest line for each header
            var lines = await _context.Support_TicketLine
             .AsNoTracking()
             .Where(l => headerUuids.Contains(l.HeaderUUID))
             .GroupBy(l => l.HeaderUUID)
             .Select(g => g.OrderBy(l => l.CreatedAt).FirstOrDefault())
             .ToListAsync();

            var result = new List<(Support_TicketHeader, Support_TicketLine)>();
            foreach (var header in headers)
            {
                var line = lines.FirstOrDefault(l => l?.HeaderUUID == header.UUID);
                result.Add((header, line));
            }

            return result;
        }

        public async Task<PagedResult<Support_TicketHeader>> GetPagedTicketHeadersByUserAsync(
            string userUuid,
            bool? isActive,
            PaginationParams pagination,
            string? search = null,
            string? sortColumn = null,
            string? sortOrder = null)
        {
            if (string.IsNullOrWhiteSpace(userUuid))
                throw new ArgumentException("User UUID cannot be null or empty.", nameof(userUuid));

            pagination ??= new PaginationParams();

            var normalizedSearch = search?.Trim().ToLowerInvariant();
            var sortKey = sortColumn?.Trim().ToLowerInvariant();
            var isAsc = string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase);

            IQueryable<Support_TicketHeader> query = _context.Support_TicketHeader
                .AsNoTracking()
                .Where(h => h.UserUUID == userUuid)
                .Select(h => new Support_TicketHeader
                {
                    Id = h.Id,
                    UUID = h.UUID,
                    TicketNumber = h.TicketNumber,
                    Subject = h.Subject,
                    Otp = h.Otp,
                    TicketCategoryUUID = h.TicketCategoryUUID,
                    CreatedAt = h.CreatedAt,
                    UserType = h.UserType,
                    UserUUID = h.UserUUID,
                    UpdatedOn = h.UpdatedOn,
                    AssigneeUUID = h.AssigneeUUID,
                    IsActive = h.IsActive,
                    Message = _context.Support_TicketLine
                        .AsNoTracking()
                        .Where(l => l.HeaderUUID == h.UUID && l.IsActive)
                        .OrderByDescending(l => l.CreatedAt)
                        .Select(l => l.Message)
                        .FirstOrDefault(),
                     
                                });

            if (isActive.HasValue)
                query = query.Where(x => x.IsActive == isActive.Value);

            if (!string.IsNullOrWhiteSpace(normalizedSearch))
            {
                query = query.Where(x =>
                    (x.TicketNumber != null && x.TicketNumber.ToLower().Contains(normalizedSearch)) ||
                    (x.Subject != null && x.Subject.ToLower().Contains(normalizedSearch)) ||
                    (x.Otp != null && x.Otp.ToLower().Contains(normalizedSearch)) ||
                    (x.Message != null && x.Message.ToLower().Contains(normalizedSearch)));
            }

            var totalCount = await query.CountAsync();

            query = sortKey switch
            {
                "ticketnumber" => isAsc ? query.OrderBy(x => x.TicketNumber) : query.OrderByDescending(x => x.TicketNumber),
                "subject" => isAsc ? query.OrderBy(x => x.Subject) : query.OrderByDescending(x => x.Subject),
                "otp" => isAsc ? query.OrderBy(x => x.Otp) : query.OrderByDescending(x => x.Otp),
                "message" => isAsc ? query.OrderBy(x => x.Message) : query.OrderByDescending(x => x.Message),
                "createdon" or "createdat" => isAsc ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
                "updatedon" or "updatedon" => isAsc ? query.OrderBy(x => x.UpdatedOn) : query.OrderByDescending(x => x.UpdatedOn),
                _ => query.OrderByDescending(x => x.UpdatedOn)
            };

            var items = await query
                .Skip(pagination.Skip)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagedResult<Support_TicketHeader>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }
}