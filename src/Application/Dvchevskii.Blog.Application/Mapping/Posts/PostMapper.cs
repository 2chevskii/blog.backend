using Dvchevskii.Blog.Application.Contracts.Entities.Posts;
using Dvchevskii.Blog.Core.Entities.Posts;

namespace Dvchevskii.Blog.Application.Mapping.Posts;

public static class PostMapper
{
    public static PostDto MapDto(Post post)
    {
        return new PostDto
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
        };
    }
}
