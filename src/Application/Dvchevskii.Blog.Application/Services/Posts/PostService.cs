using Dvchevskii.Blog.Application.Contracts.Entities.Posts;
using Dvchevskii.Blog.Application.Contracts.Services.Posts;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;
using Dvchevskii.Blog.Application.Mapping.Posts;
using Dvchevskii.Blog.Core.Entities.Posts;
using Dvchevskii.Blog.Core.Repositories.Posts;

namespace Dvchevskii.Blog.Application.Services.Posts;

internal class PostService(IPostRepository repository, IPostSlugService postSlugService) : IPostService
{
    public async Task<PostDto> Get(Guid id)
    {
        var post = await repository.GetById(id);
        return PostMapper.MapDto(post);
    }

    public async Task<PostDto> Get(string slug, bool onlyPublished)
    {
        var post = await repository.GetBySlug(slug, onlyPublished);
        return PostMapper.MapDto(post);
    }

    public async Task<LimitedQueryResult<PostDto>> GetList(LimitedQuerySettings settings, bool onlyPublished)
    {
        var posts = await repository.GetList(settings, onlyPublished, post => new PostDto
        {
            Id = post.Id,
            Slug = post.Slug,
            Title = post.Title,
            Body = post.Body,
            IsPublished = post.IsPublished,
            HeaderImageId = post.HeaderImageId,
            CreatedAt = post.AuditInfo.CreatedAt,
            CreatedBy = post.AuditInfo.CreatedBy,
            UpdatedAt = post.AuditInfo.UpdatedAt,
            UpdatedBy = post.AuditInfo.UpdatedBy,
        });

        return posts;
    }

    public async Task<PostDto> Create(CreatePostDto dto)
    {
        var slug = await postSlugService.GetAvailableSlug(dto.Title);
        dto.Slug = slug;
        var post = Post.Create(dto);

        await repository.Create(post);

        return PostMapper.MapDto(post);
    }

    public async Task<PostDto> Update(UpdatePostDto dto)
    {
        var post = await repository.GetById(dto.Id);

        if (dto.Title != post.Title)
        {
            var slug = await postSlugService.GetAvailableSlug(post.Title);
            dto.Slug = slug;
        }
        else
        {
            dto.Slug = post.Slug;
        }

        post.Update(dto);
        await repository.Update(post);

        return PostMapper.MapDto(post);
    }

    public async Task Delete(Guid id)
    {
        var post = await repository.GetById(id);
        await repository.Delete(post);
    }
}
