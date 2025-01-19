namespace Dvchevskii.Blog.Application.Contracts.Entities.Posts;

public class PostFeedEntryDto
{
    public required Guid Id { get; init; }
    public required string Slug { get; init; }
    public required PostEditorDto LastModifiedBy { get; init; }
    public required string Title { get; init; }
    public required Uri? HeaderImageUrl { get; init; }
    public required string BodyPreview { get; init; }
}
