using Dvchevskii.Blog.Entities.Common;
using Dvchevskii.Blog.Entities.Files;

namespace Dvchevskii.Blog.Entities.Posts;

public sealed class Post : Entity
{
    public required string Slug { get; set; }
    public required string Title { get; set; }
    public required string? Tagline { get; set; }
    public required string Body { get; set; }
    public required bool IsPublished { get; set; }

    public required Guid? HeaderImageId { get; set; }

    public Image? HeaderImage { get; set; }
}
