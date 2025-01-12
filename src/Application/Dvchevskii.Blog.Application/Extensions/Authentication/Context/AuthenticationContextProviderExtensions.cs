using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Context;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Context;

namespace Dvchevskii.Blog.Application.Extensions.Authentication.Context;

public static class AuthenticationContextProviderExtensions
{
    public static IAuthenticationScope CreateSystemScope(this IAuthenticationContextProvider self)
    {
        return self.CreateScope(KnownUsers.System.ToAuthenticationData());
    }
}
