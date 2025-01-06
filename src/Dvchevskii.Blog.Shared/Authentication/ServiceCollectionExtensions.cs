using Dvchevskii.Blog.Infrastructure;
using Dvchevskii.Blog.Shared.Authentication.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace Dvchevskii.Blog.Shared.Authentication;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedDataProtection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDataProtection()
            .SetApplicationName("Blog")
            .PersistKeysToDbContext<BlogDbContext>();

        return serviceCollection;
    }

    public static IServiceCollection AddBlogAuthenticationScheme(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(cookie => { cookie.Cookie.Name = "dvch.blog.auth"; });

        return serviceCollection;
    }
}
