using Dvchevskii.Blog.Core.Entities.Authentication.Users;
using Dvchevskii.Blog.Core.Entities.Common;

namespace Dvchevskii.Blog.Core.Entities.Authentication.Accounts;

public sealed class PasswordHistoryEntry : Entity
{
    public required Guid UserId { get; init; }
    public required byte[] PasswordHash { get; init; }

    public User? User { get; set; }
}
