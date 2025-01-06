using Amazon.S3;
using Amazon.S3.Model;
using Dvchevskii.Blog.Entities.Files;
using Dvchevskii.Blog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Admin.Services;

internal class ImageService(BlogDbContext dbContext, IAmazonS3 s3)
{
    private const string DevBucketName = "blog-dev";

    public async Task<Image> Create(Stream data)
    {
        var s3Key = "img/" + Guid.NewGuid().ToString("N");

        await s3.PutObjectAsync(new PutObjectRequest
        {
            Key = s3Key,
            InputStream = data,
            BucketName = DevBucketName,
        });

        var image = new Image
        {
            Id = Guid.NewGuid(),
            S3Key = s3Key,
        };
        dbContext.Add(image);
        await dbContext.SaveChangesAsync();

        return image;
    }

    public async Task<Image> Get(Guid id)
    {
        var image = await dbContext.Images.FirstAsync(x => x.Id == id);
        return image;
    }

    public async Task<Uri> GetPreSignedUrl(Guid id)
    {
        var image = await Get(id);
        var strUrl = await s3.GetPreSignedURLAsync(new GetPreSignedUrlRequest
        {
            Key = image.S3Key,
            BucketName = DevBucketName,
        });

        return new Uri(strUrl);
    }
}
