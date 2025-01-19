using Dvchevskii.Blog.Application.Contracts.Entities.Posts;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;

namespace Dvchevskii.Blog.Application.Contracts.Services.Posts;

public interface IPostReaderService
{
    Task<LimitedQueryResult<PostFeedEntryDto>> GetFeed(LimitedQuerySettings settings);
    Task<PostReadDto> Get(string slug);
}
