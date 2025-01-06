using Dvchevskii.Blog.Entities.Authentication.Users;
using Dvchevskii.Blog.Entities.Common;

namespace Dvchevskii.Blog.Entities.Authentication.Accounts;

public sealed class PasswordAccount : Entity
{
    public required bool IsDeactivated { get; set; }
    public required Guid UserId { get; init; }
    public required byte[] PasswordHash { get; set; }

    public User User { get; set; }
}
