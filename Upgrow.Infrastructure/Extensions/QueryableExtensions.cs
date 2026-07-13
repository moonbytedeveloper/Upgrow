using Microsoft.EntityFrameworkCore;
using Upgrow.Application.DTOs;
using Upgrow.Domain.Common;

namespace Upgrow.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PaginationParams pagination,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        return await query.ToPagedResultAsync(pagination, totalCount, cancellationToken);
    }

    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PaginationParams pagination,
        int totalCount,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;
        var pageSize = pagination.PageSize < 1 ? 10 : pagination.PageSize;

        var skip = pagination is DataTableRequest dt
            ? dt.Start
            : pagination.Skip;

        IReadOnlyList<T> items = totalCount == 0
            ? Array.Empty<T>()
            : await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
