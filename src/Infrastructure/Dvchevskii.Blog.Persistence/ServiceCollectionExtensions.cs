using Dvchevskii.Blog.Core.Repositories.Authentication.Accounts;
using Dvchevskii.Blog.Core.Repositories.Authentication.Users;
using Dvchevskii.Blog.Core.Repositories.Files.Images;
using Dvchevskii.Blog.Core.Repositories.Posts;
using Dvchevskii.Blog.Infrastructure.Repositories.Authentication.Passwords;
using Dvchevskii.Blog.Infrastructure.Repositories.Authentication.Users;
using Dvchevskii.Blog.Infrastructure.Repositories.Files.Images;
using Dvchevskii.Blog.Infrastructure.Repositories.Posts;
using Microsoft.AspNetCore.DataProtection;
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
                    mysql => { mysql.EnableStringComparisonTranslations(); }
                )
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(serviceProvider.GetRequiredService<AuditInfoInterceptor>())
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
        );
        serviceCollection.AddScoped<AuditInfoInterceptor>();
        return serviceCollection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IPasswordAccountRepository, PasswordAccountRepository>();
        serviceCollection.AddScoped<IPostRepository, PostRepository>();
        serviceCollection.AddScoped<IImageRepository, ImageRepository>();

        return serviceCollection;
    }

    public static IServiceCollection ConfigurePersistedDataProtection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDataProtection()
            .SetApplicationName("com.dvchevskii.blog")
            .PersistKeysToDbContext<BlogDbContext>();
        return serviceCollection;
    }
}
