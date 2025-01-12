namespace Dvchevskii.Blog.Assets.Contracts.Entities.Files.Images;

public class ImageDto
{
    public required Guid Id { get; init; }
    public required string S3Key { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required Guid CreatedBy { get; init; }
}
