using Dvchevskii.Blog.Application.Contracts.Entities.Posts;

namespace Dvchevskii.Blog.Application.Contracts.Services.Posts;

public interface IPostService
{
    Task<PostDto> Create(CreatePostDto dto);
}
