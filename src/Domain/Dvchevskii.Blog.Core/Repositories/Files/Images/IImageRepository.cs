using Dvchevskii.Blog.Core.Entities.Files;
using Dvchevskii.Blog.Core.Entities.Files.Images;

namespace Dvchevskii.Blog.Core.Repositories.Files.Images;

public interface IImageRepository
{
    Task<Image> GetById(Guid id);
    Task<string> GetS3KeyById(Guid id);
    Task<Dictionary<Guid, string>> GetS3KeysMap(IEnumerable<Guid> ids);
    Task<Image> Create(Image image);
    Task Delete(Image image);
}
