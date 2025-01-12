namespace Dvchevskii.Blog.Assets.Contracts.Entities.Files.Images;

public class UploadImageDto
{
    public required Stream Data { get; init; }
    public required string ContentType { get; init; }
}
