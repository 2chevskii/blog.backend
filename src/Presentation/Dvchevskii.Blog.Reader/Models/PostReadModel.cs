namespace Dvchevskii.Blog.Reader.Models;

public class PostReadModel
{
    public required Guid Id { get; init; }
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string? Tagline { get; init; }
    public required string Body { get; init; }
    public Uri? HeaderImageUrl { get; init; }

    public required PostAuthorModel LastModifiedBy { get; init; }
}
