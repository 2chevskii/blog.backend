namespace Dvchevskii.Blog.Application.Contracts.Services.Files.Images;

public interface IImageServiceClient
{
    Task<Uri> GetUrl(Guid id);
    Task<Dictionary<Guid, Uri>> GetUrls(IEnumerable<Guid> ids);
}

public class ImageServiceClientOptions
{
    public Uri Url { get; set; }
}
