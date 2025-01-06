namespace Dvchevskii.Blog.Shared.Contracts.Assets.Images;

public record ImageAssetDto(Guid Id, string S3Key, DateTime CreatedAt, Guid CreatedBy);
