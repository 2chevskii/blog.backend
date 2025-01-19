namespace Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;

public record LimitedQuerySettings(int Offset = 0, int Limit = 0)
{
    public IQueryable<T> Apply<T>(IQueryable<T> query)
    {
        if (Offset > 0)
        {
            query = query.Skip(Offset);
        }

        if (Limit > 0)
        {
            query = query.Take(Limit);
        }

        return query;
    }
}
