namespace Dvchevskii.Blog.Application.Contracts.Services.Files;

public interface IS3Service
{
    Task<string> UploadImage(Stream dataStream, string contentType);
    Task<Uri> GetImageUrl(string key);
}
