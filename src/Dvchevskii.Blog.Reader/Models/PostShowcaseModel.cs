namespace Dvchevskii.Blog.Reader.Models;

public class PostShowcaseModel
{
    public required Guid Id { get; init; }
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string? Tagline { get; init; }
    public required Uri? HeaderImageUrl { get; set; }
}
