using Dvchevskii.Blog.Application.Contracts.Entities.Posts;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;

namespace Dvchevskii.Blog.Application.Contracts.Services.Posts;

public interface IPostService
{
    Task<PostDto> Create(CreatePostDto dto);
    Task<PostDto> Update(UpdatePostDto dto);
    Task Delete(Guid id);
    Task<PostDto> Get(Guid id);
    Task<LimitedQueryResult<PostDto>> GetList(LimitedQuerySettings settings, bool onlyPublished);
    Task<PostDto> Get(string slug, bool onlyPublished);
}
