using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Dvchevskii.Blog.Application.Extensions.Mvc;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureLowercaseRoutes(this IServiceCollection serviceCollection)
    {
        serviceCollection.Configure<RouteOptions>(r =>
        {
            r.LowercaseUrls = true;
            r.LowercaseQueryStrings = true;
        });
        return serviceCollection;
    }

    public static IServiceCollection ConfigureJsonHandling(this IServiceCollection serviceCollection)
    {
        serviceCollection.Configure<JsonOptions>(
            json => json.JsonSerializerOptions.Converters.Add(
                new JsonStringEnumConverter()
            )
        );
        return serviceCollection;
    }
}
