using Amazon.S3;
using Amazon.S3.Model;
using Dvchevskii.Blog.Application.Contracts.Services.Files;

namespace Dvchevskii.Blog.Assets.Services.Files.S3;

internal class S3Service(IAmazonS3 amazonS3) : IS3Service
{
    private const string BucketName = "blog-dev";

    public async Task<string> UploadImage(Stream dataStream, string contentType)
    {
        var key = GetImageKey();

        var putRequest = new PutObjectRequest
        {
            BucketName = BucketName,
            ContentType = contentType,
            Key = key,
            InputStream = dataStream,
        };
        await amazonS3.PutObjectAsync(putRequest);

        return key;
    }

    public async Task<Uri> GetImageUrl(string key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = BucketName,
            Protocol = Protocol.HTTP,
            Key = key,
            Expires = DateTime.UtcNow.AddHours(1),
        };
        var strUri = await amazonS3.GetPreSignedURLAsync(request);

        return new Uri(strUri);
    }

    private static string GetImageKey()
    {
        return "images/" + Guid.NewGuid().ToString("N");
    }
}
