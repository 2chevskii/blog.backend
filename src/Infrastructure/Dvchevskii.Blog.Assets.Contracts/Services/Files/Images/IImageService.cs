using Dvchevskii.Blog.Assets.Contracts.Entities.Files.Images;

namespace Dvchevskii.Blog.Assets.Contracts.Services.Files.Images;

public interface IImageService
{
    Task<ImageDto> Upload(UploadImageDto dto);
    Task<Uri> GetUrl(Guid id);
    Task<Dictionary<Guid, Uri>> GetUrls(IEnumerable<Guid> ids);
}
