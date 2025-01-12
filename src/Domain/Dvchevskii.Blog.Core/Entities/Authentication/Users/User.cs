using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;
using Dvchevskii.Blog.Core.Entities.Authentication.Accounts;
using Dvchevskii.Blog.Core.Entities.Common;
using Dvchevskii.Blog.Core.Entities.Files;
using Dvchevskii.Blog.Core.Entities.Files.Images;

namespace Dvchevskii.Blog.Core.Entities.Authentication.Users;

public sealed class User : Entity
{
    public required bool IsBanned { get; set; }
    public required bool IsAdmin { get; set; }
    public required string? Username { get; set; }
    public required Guid? AvatarImageId { get; set; }

    public Image? AvatarImage { get; set; }
    public PasswordAccount? PasswordAccount { get; set; }
    public ICollection<PasswordHistoryEntry> PasswordHistoryEntries { get; set; }

    public static User Create(CreateUserDto dto)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            IsAdmin = dto.IsAdmin,
            IsBanned = dto.IsBanned,
            AvatarImageId = dto.AvatarImageId,
        };
    }

    public User Update(UpdateUserDto dto)
    {
        Username = dto.Username;
        IsAdmin = dto.IsAdmin;
        IsBanned = dto.IsBanned;
        AvatarImageId = dto.AvatarImageId;

        return this;
    }
}
