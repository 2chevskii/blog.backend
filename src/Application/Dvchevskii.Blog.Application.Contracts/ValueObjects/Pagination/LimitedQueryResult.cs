namespace Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;

public class LimitedQueryResult<T>
{
    public List<T> Items { get; set; }
    public LimitedQuerySettings Settings { get; set; }
    public int TotalCount { get; set; }

    public LimitedQueryResult<R> Map<R>(Converter<T, R> mapper)
    {
        return new LimitedQueryResult<R>
        {
            Items = Items.ConvertAll(mapper),
            Settings = Settings,
            TotalCount = TotalCount,
        };
    }

    public async ValueTask<LimitedQueryResult<R>> MapAsync<R>(Converter<T, ValueTask<R>> mapper)
    {
        var items = new List<R>();

        foreach (var item in Items)
        {
            items.Add(await mapper(item));
        }

        return new LimitedQueryResult<R>
        {
            Items = items,
            Settings = Settings,
            TotalCount = TotalCount,
        };
    }
}
