using System.Text;
using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Passwords;
using Dvchevskii.Blog.Core.Entities.Authentication.Users;
using Dvchevskii.Blog.Core.Entities.Common;

namespace Dvchevskii.Blog.Core.Entities.Authentication.Accounts;

public sealed class PasswordAccount : Entity
{
    public required bool IsDeactivated { get; set; }
    public required Guid UserId { get; init; }
    public required byte[] PasswordHash { get; set; }

    public User User { get; set; }

    public static PasswordAccount Create(CreatePasswordAccountDto dto)
    {
        if (dto.HashedValue is not { Length: not 0 })
        {
            throw new ArgumentException("Hashed value is empty");
        }

        return new PasswordAccount
        {
            Id = Guid.NewGuid(),
            IsDeactivated = false,
            UserId = dto.User.Id,
            PasswordHash = dto.HashedValue,
        };
    }

    public PasswordAccount Update(UpdatePasswordAccountDto dto)
    {
        if (dto.HashedValue is not { Length: not 0 })
        {
            throw new ArgumentException("Hashed value is empty");
        }

        PasswordHash = dto.HashedValue;
        return this;
    }

    public PasswordAccount SetDeactivated(bool deactivated = true)
    {
        IsDeactivated = deactivated;
        return this;
    }
}
