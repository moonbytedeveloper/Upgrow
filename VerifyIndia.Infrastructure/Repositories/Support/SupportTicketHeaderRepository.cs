using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories.Support
{
    public class SupportTicketHeaderRepository : MasterRepositoryBase<Support_TicketHeader>
    {
        public SupportTicketHeaderRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<Support_TicketHeader>> GetPagedAsync(
    Expression<Func<Support_TicketHeader, bool>>? filter,
    PaginationParams pagination,
    Func<IQueryable<Support_TicketHeader>, IOrderedQueryable<Support_TicketHeader>>? orderBy = null,
    Func<IQueryable<Support_TicketHeader>, IQueryable<Support_TicketHeader>>? queryModifier = null)
        {
            IQueryable<Support_TicketHeader> query = _context.Support_TicketHeader.AsNoTracking();

            if (queryModifier != null)
                query = queryModifier(query);

            if (filter != null)
                query = query.Where(filter);

            var total = await query.CountAsync();

            query = orderBy != null
                ? orderBy(query)
                : query.OrderBy(x => x.Id);

            var pageQuery = query
                .Skip(pagination.Skip)
                .Take(pagination.PageSize);

            var items = await pageQuery
                .Select(h => new Support_TicketHeader
                {
                    Id = h.Id,
                    UUID = h.UUID,
                    TicketNumber = h.TicketNumber,
                    Subject = h.Subject,
                    Message = _context.Support_TicketLine
                    .Where(s => s.HeaderUUID == h.UUID)
                    .Select(s => s.Message)
                    .FirstOrDefault(),
                    UserType = h.UserType,
                    CreatedAt = h.CreatedAt,
                    UpdatedOn =h.UpdatedOn,
                    IsActive = h.IsActive,

                    TicketCategoryUUID = _context.Support_TicketCategory
                        .Where(d => d.UUID == h.TicketCategoryUUID)
                        .Select(d => d.Title)
                        .FirstOrDefault() ?? h.TicketCategoryUUID,

                    UserUUID = _context.Master_Employee
                        .Where(e => e.UUID == h.UserUUID)
                        .Select(e => ((e.FirstName ?? "") + (e.LastName != null ? " " + e.LastName : "")).Trim())
                        .FirstOrDefault() ?? h.UserUUID,

                    AssigneeUUID = _context.Master_Employee
                        .Where(e => e.UUID == h.AssigneeUUID)
                        .Select(e => ((e.FirstName ?? "") + (e.LastName != null ? " " + e.LastName : "")).Trim())
                        .FirstOrDefault() ?? h.AssigneeUUID,

                    RaisedByName = (h.UserType ?? "").ToLower() == "whitelabel"
                        ? (_context.Tenant
                            .Where(t => t.UUID == h.UserUUID)
                            .Select(t => t.TenantName)
                            .FirstOrDefault() ?? h.UserUUID)
                        : (_context.Master_Customer
                            .Where(c => c.UUID == h.UserUUID)
                            .Select(c => ((c.FName ?? "") + (c.LName != null ? " " + c.LName : "")).Trim())
                            .FirstOrDefault() ?? h.UserUUID),

                    RaisedByMobile = (h.UserType ?? "").ToLower() == "whitelabel"
                        ? _context.Tenant
                            .Where(t => t.UUID == h.UserUUID)
                            .Select(t => t.Mobile)
                            .FirstOrDefault()
                        : _context.Master_Customer
                            .Where(c => c.UUID == h.UserUUID)
                            .Select(c => c.Mobile)
                            .FirstOrDefault(),

                    RaisedByEmail = (h.UserType ?? "").ToLower() == "whitelabel"
                        ? _context.Tenant
                            .Where(t => t.UUID == h.UserUUID)
                            .Select(t => t.Email)
                            .FirstOrDefault()
                        : _context.Master_Customer
                            .Where(c => c.UUID == h.UserUUID)
                            .Select(c => c.Email)
                            .FirstOrDefault()
                })
                .ToListAsync();

            return new PagedResult<Support_TicketHeader>
            {
                Items = items,
                TotalCount = total,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }
}