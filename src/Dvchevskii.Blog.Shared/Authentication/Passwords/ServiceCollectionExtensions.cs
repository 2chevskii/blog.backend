using Microsoft.Extensions.DependencyInjection;

namespace Dvchevskii.Blog.Shared.Authentication.Passwords;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPasswordHasher(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<PasswordHasher>();
        return serviceCollection;
    }
}
