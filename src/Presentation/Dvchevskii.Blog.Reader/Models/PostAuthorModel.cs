namespace Dvchevskii.Blog.Reader.Models;

public class PostAuthorModel
{
    public required Guid Id { get; init; }
    public required string? Username { get; init; }
    public required DateTime Timestamp { get; init; }
    public Uri? AvatarUrl { get; set; }
}
