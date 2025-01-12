namespace Dvchevskii.Blog.Application.Contracts.Entities.Posts;

public class UpdatePostDto
{
    public required Guid Id { get; init; }

    public string Slug { get; set; }
    public required string Title { get; init; }
    public required string Body { get; init; }
    public required bool IsPublished { get; init; }
    public required Guid? HeaderImageId { get; init; }
}
