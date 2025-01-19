using Dvchevskii.Blog.Application.Contracts.Services.Authentication;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Context;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Passwords;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.Services.Files.Images;
using Dvchevskii.Blog.Application.Contracts.Services.Posts;
using Dvchevskii.Blog.Application.Contracts.Services.Setup;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Context;
using Dvchevskii.Blog.Application.Middleware.Authentication.Context;
using Dvchevskii.Blog.Application.Services.Authentication;
using Dvchevskii.Blog.Application.Services.Authentication.Context;
using Dvchevskii.Blog.Application.Services.Authentication.Local;
using Dvchevskii.Blog.Application.Services.Authentication.Passwords;
using Dvchevskii.Blog.Application.Services.Authentication.Users;
using Dvchevskii.Blog.Application.Services.Files.Images;
using Dvchevskii.Blog.Application.Services.Posts;
using Dvchevskii.Blog.Application.Services.Setup;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dvchevskii.Blog.Application;

public static class ApplicationConfigurator
{
    public static void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IPostSlugService, PostSlugService>();
        serviceCollection.AddScoped<IPostService, PostService>();

        serviceCollection.AddSingleton<IAuthenticationContextProvider, AuthenticationContextProvider>();
        serviceCollection.AddSingleton<IAuthenticationContext>(sp =>
            sp.GetRequiredService<IAuthenticationContextProvider>().Context
        );
        serviceCollection.AddScoped<IAuthenticationScope>(sp =>
            sp.GetRequiredService<IAuthenticationContextProvider>().CreateScope()
        );

        serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => { options.Cookie.Name = "dvchevskii.blog.authentication"; });

        serviceCollection.AddScoped<AuthenticationContextSetterMiddleware>();

        serviceCollection.AddScoped<LocalAuthenticationService>();
        serviceCollection.AddScoped<IBlogAuthenticationService, BlogAuthenticationService>();
        serviceCollection.AddScoped<IPasswordAccountService, PasswordAccountService>();
        serviceCollection.AddSingleton<PasswordHashService>();

        serviceCollection.AddScoped<IUserService, UserService>();

        serviceCollection.AddHostedService<SetupRunner>();

        serviceCollection.AddScoped<IImageServiceClient, ImageServiceClient>();
        serviceCollection.AddOptions<ImageServiceClientOptions>()
            .Configure(options => options.Url = new Uri("http://localhost:3102/"));

        serviceCollection.AddScoped<IPostAdminService, PostAdminService>();
        serviceCollection.AddScoped<IUserAvatarService, UserAvatarService>();

        serviceCollection.AddScoped<IPostReaderService, PostReaderService>();
    }

    public static void Configure(IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseMiddleware<AuthenticationContextSetterMiddleware>();
        app.UseAuthorization();
    }

    public static IServiceCollection AddSetupHandlers(
        this IServiceCollection serviceCollection,
        Predicate<Type>? filter = null
    )
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsAssignableTo(typeof(ISetupHandler)) && type is { IsClass: true, IsAbstract: false })
            .Where(type => filter?.Invoke(type) ?? true)
            .ToArray();

        var serviceDescriptors = types.Select(type =>
            ServiceDescriptor.Describe(typeof(ISetupHandler), type, ServiceLifetime.Scoped)
        ).ToList();

        serviceDescriptors.ForEach(serviceCollection.TryAddEnumerable);

        return serviceCollection;
    }
}
