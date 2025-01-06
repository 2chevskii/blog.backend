namespace Dvchevskii.Blog.Auth.Models;

public enum LocalSignInResultType
{
    OK,
    USER_NOT_FOUND,
    PASSWORD_ACCOUNT_NOT_FOUND,
    PASSWORD_ACCOUNT_DEACTIVATED,
    PASSWORD_INVALID,
    USER_BANNED,
}