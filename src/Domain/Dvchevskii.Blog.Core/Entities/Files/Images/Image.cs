using System.ComponentModel.DataAnnotations.Schema;
using Dvchevskii.Blog.Assets.Contracts.Entities.Files.Images;
using Dvchevskii.Blog.Core.Entities.Common;

namespace Dvchevskii.Blog.Core.Entities.Files.Images;

public sealed class Image : Entity
{
    [Column("s3_key")] public required string S3Key { get; init; }

    public static Image Create(CreateImageDto dto)
    {
        return new Image
        {
            Id = Guid.NewGuid(),
            S3Key = dto.S3Key,
        };
    }
}
