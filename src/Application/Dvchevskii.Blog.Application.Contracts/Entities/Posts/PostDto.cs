namespace Dvchevskii.Blog.Application.Contracts.Entities.Posts;

public class PostDto
{
    public required Guid Id { get; init; }
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string Body { get; init; }
    public required Guid? HeaderImageId { get; init; }
    public required bool IsPublished { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required Guid CreatedBy { get; init; }
    public required DateTime? UpdatedAt { get; init; }
    public required Guid? UpdatedBy { get; init; }
}
