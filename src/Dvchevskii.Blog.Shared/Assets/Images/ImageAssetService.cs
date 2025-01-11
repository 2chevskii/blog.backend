using Dvchevskii.Blog.Shared.Contracts.Assets.Images;

namespace Dvchevskii.Blog.Shared.Assets.Images;

internal class ImageAssetService(ImageAssetApiClient apiClient) : IImageAssetService
{
    public Task<ImageAssetDto?> Find(Guid id)
    {
        return apiClient.GetImageAsset(id);
    }

    public Task<Uri?> GetPreSignedUrl(Guid id)
    {
        return apiClient.GetUrl(id);
    }

    public Task<Dictionary<Guid, Uri>> GetPreSignedUrlList(IEnumerable<Guid> ids)
    {
        return apiClient.GetUrls(ids);
    }
}
