namespace Dvchevskii.Blog.Shared.Contracts.Authentication.Context;

public interface IAuthenticationContextProvider
{
    IAuthenticationContext Context { get; }

    /// <summary>
    /// Create <see cref="IAuthenticationScope"/> with given authentication info
    /// </summary>
    /// <param name="authenticationData"></param>
    /// <returns></returns>
    IAuthenticationScope CreateScope(AuthenticationData authenticationData);

    /// <summary>
    /// Create <see cref="IAuthenticationScope"/> with authentication info derived from current context
    /// </summary>
    /// <returns></returns>
    IAuthenticationScope CreateScope();
}
