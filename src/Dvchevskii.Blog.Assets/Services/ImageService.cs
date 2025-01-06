using Amazon.S3;
using Amazon.S3.Model;
using Dvchevskii.Blog.Entities.Files;
using Dvchevskii.Blog.Infrastructure;
using Dvchevskii.Blog.Shared.Contracts.Assets.Images;
using Dvchevskii.Blog.Shared.Contracts.Authentication.Context;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Assets.Services;

internal class ImageService(BlogDbContext dbContext, IAmazonS3 s3, IAuthenticationScope authenticationScope)
{
    private const string DevBucketName = "blog-dev";

    public async Task<ImageAssetDto> Create(Stream data, string contentType)
    {
        if (!authenticationScope.IsAuthenticated)
        {
            throw new Exception("Not authenticated");
        }

        var s3Key = "img/" + Guid.NewGuid().ToString("N");

        await s3.PutObjectAsync(new PutObjectRequest
        {
            Key = s3Key,
            InputStream = data,
            BucketName = DevBucketName,
            ContentType = contentType,
        });

        var image = new Image
        {
            Id = Guid.NewGuid(),
            S3Key = s3Key,
        };
        dbContext.Add(image);
        await dbContext.SaveChangesAsync();

        return new ImageAssetDto(image.Id, image.S3Key, image.AuditInfo.CreatedAt, image.AuditInfo.CreatedBy);
    }

    public async Task<ImageAssetDto?> Find(Guid id)
    {
        var image = await dbContext.Images.FirstOrDefaultAsync(x => x.Id == id);

        if (image == null)
        {
            return null;
        }

        return new ImageAssetDto(image.Id, image.S3Key, image.AuditInfo.CreatedAt, image.AuditInfo.CreatedBy);
    }

    public async Task<Uri?> GetPreSignedUrl(Guid id)
    {
        var image = await Find(id);

        if (image == null)
        {
            return null;
        }

        var strUrl = await s3.GetPreSignedURLAsync(new GetPreSignedUrlRequest
        {
            Key = image.S3Key,
            BucketName = DevBucketName,
            Expires = DateTime.UtcNow.AddHours(1),
            Protocol = Protocol.HTTP,
        });

        return new Uri(strUrl);
    }
}
