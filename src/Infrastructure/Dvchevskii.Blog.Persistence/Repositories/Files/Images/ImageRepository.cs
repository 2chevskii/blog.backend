using Dvchevskii.Blog.Core.Entities.Files.Images;
using Dvchevskii.Blog.Core.Repositories.Files.Images;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Infrastructure.Repositories.Files.Images;

internal class ImageRepository(BlogDbContext dbContext) : IImageRepository
{
    public async Task<Image> GetById(Guid id)
    {
        var image = await dbContext.Images.FirstAsync(x => x.Id == id);
        return image;
    }

    public async Task<string> GetS3KeyById(Guid id)
    {
        var key = await dbContext.Images.Where(x => x.Id == id)
            .Select(x => x.S3Key)
            .FirstAsync();

        return key;
    }

    public async Task<Dictionary<Guid, string>> GetS3KeysMap(IEnumerable<Guid> ids)
    {
        var keys = await dbContext.Images
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.S3Key);

        return keys;
    }

    public async Task<Image> Create(Image image)
    {
        dbContext.Images.Add(image);
        await dbContext.SaveChangesAsync();
        return image;
    }

    public async Task Delete(Image image)
    {
        dbContext.Images.Remove(image);
        await dbContext.SaveChangesAsync();
    }
}
