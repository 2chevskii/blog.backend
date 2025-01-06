namespace Dvchevskii.Blog.Auth.Models;

public sealed record LocalSignInResult(LocalSignInResultType Type, LocalUserInfo? UserInfo = null);
