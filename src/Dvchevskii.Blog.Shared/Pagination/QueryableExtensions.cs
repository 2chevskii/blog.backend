using Dvchevskii.Blog.Shared.Contracts.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Shared.Pagination;

public static class QueryableExtensions
{
    public static async Task<PaginationQueryResult<T>> ToPaginatedAsync<T>(this IQueryable<T> query, int offset,
        int limit)
    {
        var totalCount = await query.CountAsync();

        var items = await query.Skip(offset)
            .Take(limit)
            .ToListAsync();

        return new PaginationQueryResult<T>
        {
            Items = items,
            Count = items.Count,
            Offset = offset,
            Limit = limit,
            TotalCount = totalCount,
        };
    }
}
