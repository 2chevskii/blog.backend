using Dvchevskii.Blog.Shared.Contracts.Authentication.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Dvchevskii.Blog.Shared.Authentication.Context;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthenticationContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IAuthenticationContextProvider, AuthenticationContextProvider>();
        serviceCollection.AddSingleton<IAuthenticationContext>(sp =>
            sp.GetRequiredService<IAuthenticationContextProvider>().Context
        );
        serviceCollection.AddScoped<IAuthenticationScope>(sp =>
            sp.GetRequiredService<IAuthenticationContextProvider>().CreateScope()
        );

        return serviceCollection;
    }

    public static IServiceCollection AddAuthenticationContextSetterMiddleware(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<AuthenticationContextSetter>();
        return serviceCollection;
    }
}

public static class WebApplicationExtensions
{
    public static WebApplication UseAuthenticationContextSetter(this WebApplication webApplication)
    {
        webApplication.UseMiddleware<AuthenticationContextSetter>();
        return webApplication;
    }
}
