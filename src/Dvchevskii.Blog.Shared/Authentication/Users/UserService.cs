using Dvchevskii.Blog.Entities.Authentication.Users;
using Dvchevskii.Blog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Shared.Authentication.Users;

internal class UserService(BlogDbContext dbContext)
{
    public async Task<UserDto> Create(CreateUserDto dto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            IsAdmin = dto.IsAdmin,
            IsBanned = dto.IsBanned,
            AvatarImageId = null,
        };
        dbContext.Add(user);
        await dbContext.SaveChangesAsync();
        return new UserDto(user.Id, user.Username, user.IsAdmin, user.IsBanned);
    }

    public async Task<UserDto> Update(UpdateUserDto dto)
    {
        var user = await dbContext.Users.FirstAsync(x => x.Id == dto.Id);

        user.Username = dto.Username;
        user.IsAdmin = dto.IsAdmin;
        user.IsBanned = dto.IsBanned;

        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        return new UserDto(user.Id, user.Username, user.IsAdmin, user.IsBanned);
    }
}

public record UpdateUserDto(Guid Id, string? Username, bool IsAdmin, bool IsBanned);

public record CreateUserDto(string? Username, bool IsAdmin, bool IsBanned);

public record UserDto(Guid Id, string? Username, bool IsAdmin, bool IsBanned);
