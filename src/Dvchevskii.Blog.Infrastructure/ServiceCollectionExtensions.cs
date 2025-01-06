using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dvchevskii.Blog.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlogDbContext(
        this IServiceCollection serviceCollection,
        string connectionString
    )
    {
        serviceCollection.AddDbContext<BlogDbContext>(
            (serviceProvider, options) => options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    mysql => { }
                )
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(serviceProvider.GetRequiredService<AuditInfoInterceptor>())
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
        );
        serviceCollection.AddScoped<AuditInfoInterceptor>();
        return serviceCollection;
    }
}
