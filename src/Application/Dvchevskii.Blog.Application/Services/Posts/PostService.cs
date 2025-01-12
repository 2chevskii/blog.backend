using Dvchevskii.Blog.Application.Contracts.Entities.Posts;
using Dvchevskii.Blog.Application.Contracts.Services.Posts;
using Dvchevskii.Blog.Application.Mapping.Posts;
using Dvchevskii.Blog.Core.Entities.Posts;
using Dvchevskii.Blog.Core.Repositories.Posts;

namespace Dvchevskii.Blog.Application.Services.Posts;

internal class PostService(IPostRepository postRepository, IPostSlugService postSlugService) : IPostService
{
    public async Task<PostDto> Create(CreatePostDto dto)
    {
        var slug = await postSlugService.GetAvailableSlug(dto.Title);
        dto.Slug = slug;
        var post = Post.Create(dto);

        await postRepository.Create(post);

        return PostMapper.MapDto(post);
    }

    public async Task<PostDto> Update(UpdatePostDto dto)
    {
        var post = await postRepository.GetById(dto.Id);

        if (dto.Title != post.Title)
        {
            var slug = await postSlugService.GetAvailableSlug(post.Title);
            dto.Slug = slug;
        }

        post.Update(dto);
        await postRepository.Update(post);

        return PostMapper.MapDto(post);
    }

    public async Task Delete(Guid id)
    {
        var post = await postRepository.GetById(id);
        await postRepository.Delete(post);
    }
}
