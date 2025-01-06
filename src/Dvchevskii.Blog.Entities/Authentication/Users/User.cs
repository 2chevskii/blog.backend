using Dvchevskii.Blog.Entities.Authentication.Accounts;
using Dvchevskii.Blog.Entities.Common;
using Dvchevskii.Blog.Entities.Files;

namespace Dvchevskii.Blog.Entities.Authentication.Users;

public sealed class User : Entity
{
    public required bool IsBanned { get; set; }
    public required bool IsAdmin { get; set; }
    public required string? Username { get; set; }
    public required Guid? AvatarImageId { get; set; }

    public Image? AvatarImage { get; set; }
    public PasswordAccount? PasswordAccount { get; set; }
    public ICollection<PasswordHistoryEntry> PasswordHistoryEntries { get; set; }
}
