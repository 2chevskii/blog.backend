namespace Dvchevskii.Blog.Shared.Contracts.Pagination;

public class PaginationQueryResult<T>
{
    public List<T> Items { get; set; }
    public int Offset { get; set; }
    public int Limit { get; set; }
    public int Count { get; set; }
    public int TotalCount { get; set; }

    public PaginationQueryResult<R> Map<R>(Converter<T, R> mapperFunc) => new PaginationQueryResult<R>
    {
        Items = Items.ConvertAll(mapperFunc),
        Offset = Offset,
        Limit = Limit,
        Count = Count,
        TotalCount = TotalCount,
    };
}
