using Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Application.Extensions.Pagination;

public static class LimitedQueryableExtensions
{
    public static async ValueTask<LimitedQueryResult<T>> ToLimitedResult<T>(
        this IQueryable<T> query,
        LimitedQuerySettings settings
    )
    {
        var totalCount = await query.CountAsync();

        var items = await settings.Apply(query).ToListAsync();

        return new LimitedQueryResult<T>
        {
            Items = items,
            Settings = settings,
            TotalCount = totalCount,
        };
    }
}
