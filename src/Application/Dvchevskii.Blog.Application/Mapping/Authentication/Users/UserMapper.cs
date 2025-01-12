using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;
using Dvchevskii.Blog.Core.Entities.Authentication.Users;

namespace Dvchevskii.Blog.Application.Mapping.Authentication.Users;

public static class UserMapper
{
    public static UserDto MapDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            IsAdmin = user.IsAdmin,
            IsBanned = user.IsBanned,
            AvatarImageId = user.AvatarImageId,
            CreatedAt = user.AuditInfo.CreatedAt,
            UpdatedAt = user.AuditInfo.UpdatedAt,
        };
    }
}
