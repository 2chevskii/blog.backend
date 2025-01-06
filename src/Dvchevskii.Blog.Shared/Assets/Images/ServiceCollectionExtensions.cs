using Dvchevskii.Blog.Shared.Contracts.Assets.Images;
using Microsoft.Extensions.DependencyInjection;

namespace Dvchevskii.Blog.Shared.Assets.Images;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageAssetServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient(nameof(ImageAssetApiClient),
            httpClient => { httpClient.BaseAddress = new Uri("http://localhost:3003/"); }
        );
        serviceCollection.AddScoped<IImageAssetService, ImageAssetService>();
        serviceCollection.AddScoped<ImageAssetApiClient>();
        return serviceCollection;
    }
}
