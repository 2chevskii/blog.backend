using Dvchevskii.Blog.Entities.Posts;
using Dvchevskii.Blog.Reader.DTOs;

namespace Dvchevskii.Blog.Reader.Extensions;

public static class PostExtensions
{
    public static PostDto ToDto(this Post post) => new PostDto
    {
        Id = post.Id,
        Slug = post.Slug,
        Title = post.Title,
        Tagline = post.Tagline,
        Body = post.Body,
        HeaderImageId = post.HeaderImageId,
        CreatedAt = post.AuditInfo.CreatedAt,
        CreatedBy = post.AuditInfo.CreatedBy,
        UpdatedAt = post.AuditInfo.UpdatedAt,
        UpdatedBy = post.AuditInfo.UpdatedBy,
    };
}
