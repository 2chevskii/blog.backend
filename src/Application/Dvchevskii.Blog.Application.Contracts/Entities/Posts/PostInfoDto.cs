namespace Dvchevskii.Blog.Application.Contracts.Entities.Posts;

public class PostInfoDto
{
    public required Guid Id { get; init; }
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required bool IsPublished { get; init; }
    public required PostEditorDto CreatedBy { get; init; }
    public required PostEditorDto? UpdatedBy { get; init; }
}
