using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Context;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Context;

namespace Dvchevskii.Blog.Application.Services.Authentication.Context;

internal class AuthenticationContextProvider : IAuthenticationContextProvider, IAuthenticationContext
{
    private static readonly AsyncLocal<AuthenticationContextHolder> CurrentHolder =
        new AsyncLocal<AuthenticationContextHolder>();

    public IAuthenticationContext Context => this;

    public bool IsAuthenticated => GetCurrentContext().IsAuthenticated;
    public Guid UserId => GetCurrentContext().UserId;
    public bool IsAdmin => GetCurrentContext().IsAdmin;
    public string? Username => GetCurrentContext().Username;

    public IAuthenticationScope CreateScope(AuthenticationData authenticationData)
    {
        var currentScope = GetCurrentContext();
        var newScope = currentScope.CreateDerived(authenticationData);
        SetCurrentContext(newScope);
        return newScope;
    }

    public IAuthenticationScope CreateScope()
    {
        var currentScope = GetCurrentContext();
        var derivedScope = currentScope.CreateDerived();
        SetCurrentContext(derivedScope);
        return derivedScope;
    }

    private static AuthenticationContext GetCurrentContext()
    {
        var holder = CurrentHolder.Value;
        if (holder != null) return holder.Value;
        holder = new AuthenticationContextHolder { Value = AuthenticationContext.CreateDefault(), };
        CurrentHolder.Value = holder;

        return holder.Value;
    }

    private static void SetCurrentContext(AuthenticationContext context)
    {
        var holder = CurrentHolder.Value;
        if (holder != null)
        {
            CurrentHolder.Value = null!;
        }

        CurrentHolder.Value = new AuthenticationContextHolder { Value = context };
    }

    private class AuthenticationContextHolder
    {
        public required AuthenticationContext Value;
    }

    private class AuthenticationContext : IAuthenticationScope
    {
        private volatile bool _isDisposed;

        public bool IsAuthenticated { get; private init; }
        public Guid UserId { get; private init; }
        public bool IsAdmin { get; private init; }
        public string? Username { get; private init; }
        private AuthenticationContext? PreviousContext { get; init; }

        public static AuthenticationContext CreateDefault() => new AuthenticationContext();

        public AuthenticationContext CreateDerived(AuthenticationData authenticationData)
        {
            return new AuthenticationContext
            {
                PreviousContext = this,
                IsAuthenticated = true,
                UserId = authenticationData.UserId,
                IsAdmin = authenticationData.IsAdmin,
                Username = authenticationData.Username,
            };
        }

        public AuthenticationContext CreateDerived()
        {
            return new AuthenticationContext
            {
                PreviousContext = this,
                IsAuthenticated = IsAuthenticated,
                UserId = UserId,
                IsAdmin = IsAdmin,
                Username = Username,
            };
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (PreviousContext != null)
            {
                SetCurrentContext(PreviousContext);
            }

            _isDisposed = true;
        }
    }
}
