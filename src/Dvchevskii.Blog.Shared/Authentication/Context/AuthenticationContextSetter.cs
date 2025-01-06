using System.Security.Claims;
using Dvchevskii.Blog.Shared.Contracts.Authentication;
using Dvchevskii.Blog.Shared.Contracts.Authentication.Context;
using Microsoft.AspNetCore.Http;

namespace Dvchevskii.Blog.Shared.Authentication.Context;

public class AuthenticationContextSetter(IAuthenticationContextProvider authenticationContextProvider) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            await next(context);
            return;
        }

        var authenticationData = new AuthenticationData
        {
            UserId = Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)!),
            IsAdmin = context.User.HasClaim(ClaimTypes.Role, "admin"),
            Username = context.User.FindFirstValue(ClaimTypes.GivenName),
        };

        using var authenticationScope = authenticationContextProvider.CreateScope(authenticationData);
        await next(context);
    }
}
