using System.ComponentModel.DataAnnotations.Schema;
using Dvchevskii.Blog.Entities.Common;

namespace Dvchevskii.Blog.Entities.Files;

public sealed class Image : Entity
{
    [Column("s3_key")] public required string S3Key { get; init; }
}
