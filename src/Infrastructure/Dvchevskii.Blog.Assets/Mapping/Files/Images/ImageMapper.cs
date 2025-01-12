using Dvchevskii.Blog.Assets.Contracts.Entities.Files.Images;
using Dvchevskii.Blog.Core.Entities.Files.Images;

namespace Dvchevskii.Blog.Assets.Mapping.Files.Images;

public static class ImageMapper
{
    public static ImageDto MapDto(Image image)
    {
        return new ImageDto
        {
            Id = image.Id,
            S3Key = image.S3Key,
            CreatedAt = image.AuditInfo.CreatedAt,
            CreatedBy = image.AuditInfo.CreatedBy,
        };
    }
}
