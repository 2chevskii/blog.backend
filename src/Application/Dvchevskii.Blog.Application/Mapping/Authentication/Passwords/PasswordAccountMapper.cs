using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Passwords;
using Dvchevskii.Blog.Core.Entities.Authentication.Accounts;

namespace Dvchevskii.Blog.Application.Mapping.Authentication.Passwords;

public static class PasswordAccountMapper
{
    public static PasswordAccountDto MapDto(PasswordAccount passwordAccount)
    {
        return new PasswordAccountDto
        {
            Id = passwordAccount.Id,
            UserId = passwordAccount.UserId,
            IsDeactivated = passwordAccount.IsDeactivated,
        };
    }
}
