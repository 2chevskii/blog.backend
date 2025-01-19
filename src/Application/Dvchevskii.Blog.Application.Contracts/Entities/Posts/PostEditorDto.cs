namespace Dvchevskii.Blog.Application.Contracts.Entities.Posts;

public class PostEditorDto
{
    public required Guid Id { get; init; }
    public required string? Username { get; init; }
    public required Uri? AvatarUrl { get; set; }
    public required DateTime Timestamp { get; init; }
}
