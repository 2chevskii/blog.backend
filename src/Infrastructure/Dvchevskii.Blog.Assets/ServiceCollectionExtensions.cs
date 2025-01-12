using Amazon.Runtime;
using Amazon.S3;
using Dvchevskii.Blog.Application.Contracts.Services.Files;
using Dvchevskii.Blog.Application.Contracts.Services.Files.Images;
using Dvchevskii.Blog.Assets.Contracts.Services.Files.Images;
using Dvchevskii.Blog.Assets.Services;
using Dvchevskii.Blog.Assets.Services.Files.Images;
using Dvchevskii.Blog.Assets.Services.Files.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dvchevskii.Blog.Assets;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddS3(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddOptions<S3Options>().Bind(configuration);

        serviceCollection.AddScoped<IAmazonS3, AmazonS3Client>(services =>
        {
            var options = services.GetRequiredService<IOptions<S3Options>>();
            var credentials = new BasicAWSCredentials(options.Value.AccessKey, options.Value.SecretKey);
            var config = new AmazonS3Config
            {
                ServiceURL = options.Value.ServiceUrl,
                ForcePathStyle = true,
            };
            var client = new AmazonS3Client(credentials, config);
            return client;
        });
        serviceCollection.AddScoped<IS3Service, S3Service>();

        return serviceCollection;
    }

    public static IServiceCollection AddImageService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IImageService, ImageService>();

        return serviceCollection;
    }
}
