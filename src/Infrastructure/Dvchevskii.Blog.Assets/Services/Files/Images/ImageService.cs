using Dvchevskii.Blog.Application.Contracts.Services.Files;
using Dvchevskii.Blog.Application.Contracts.Services.Files.Images;
using Dvchevskii.Blog.Assets.Contracts.Entities.Files.Images;
using Dvchevskii.Blog.Assets.Contracts.Services.Files.Images;
using Dvchevskii.Blog.Assets.Mapping.Files.Images;
using Dvchevskii.Blog.Core.Entities.Files.Images;
using Dvchevskii.Blog.Core.Repositories.Files.Images;

namespace Dvchevskii.Blog.Assets.Services.Files.Images;

internal class ImageService(IImageRepository imageRepository, IS3Service s3Service) : IImageService
{
    public async Task<ImageDto> Upload(UploadImageDto dto)
    {
        var key = await s3Service.UploadImage(dto.Data, dto.ContentType);
        var createImageDto = new CreateImageDto
        {
            S3Key = key,
        };
        var image = Image.Create(createImageDto);
        await imageRepository.Create(image);

        return ImageMapper.MapDto(image);
    }

    public async Task<Uri> GetUrl(Guid id)
    {
        var key = await imageRepository.GetS3KeyById(id);
        var url = await s3Service.GetImageUrl(key);
        return url;
    }

    public async Task<Dictionary<Guid, Uri>> GetUrls(IEnumerable<Guid> ids)
    {
        var keys = await imageRepository.GetS3KeysMap(ids);
        var result = new Dictionary<Guid, Uri>();
        foreach (var id in ids)
        {
            var key = keys[id];
            var url = await s3Service.GetImageUrl(key);
            result.Add(id, url);
        }

        return result;
    }


}
