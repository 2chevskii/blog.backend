using Dvchevskii.Blog.Shared.Authentication.Users;
using Dvchevskii.Blog.Shared.Contracts.Authentication.Context;
using Dvchevskii.Blog.Shared.Contracts.Authentication.Users;

namespace Dvchevskii.Blog.Shared.Authentication.Context;

public static class AuthenticationContextProviderExtensions
{
    public static IAuthenticationScope CreateSystemScope(this IAuthenticationContextProvider self)
    {
        return self.CreateScope(KnownUsers.System.ToAuthenticationData());
    }
}
