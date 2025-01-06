using Dvchevskii.Blog.Entities.Authentication.Users;
using Dvchevskii.Blog.Entities.Common;

namespace Dvchevskii.Blog.Entities.Authentication.Accounts;

public sealed class PasswordHistoryEntry : Entity
{
    public required Guid UserId { get; init; }
    public required byte[] PasswordHash { get; init; }

    public User? User { get; set; }
}
