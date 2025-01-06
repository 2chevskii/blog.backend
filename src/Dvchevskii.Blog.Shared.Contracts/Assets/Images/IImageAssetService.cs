namespace Dvchevskii.Blog.Shared.Contracts.Assets.Images;

public interface IImageAssetService
{
    Task<ImageAssetDto?> Find(Guid id);

    Task<Uri?> GetPreSignedUrl(Guid id);
}
