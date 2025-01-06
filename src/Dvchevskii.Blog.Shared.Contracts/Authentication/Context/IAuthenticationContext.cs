namespace Dvchevskii.Blog.Shared.Contracts.Authentication.Context;

public interface IAuthenticationContext
{
    bool IsAuthenticated { get; }

    Guid UserId { get; }
    bool IsAdmin { get; }
    string? Username { get; }
}
