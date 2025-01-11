using Dvchevskii.Blog.Infrastructure;
using Dvchevskii.Blog.Reader.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Reader.Services;

public class UserService(BlogDbContext dbContext)
{
    public async Task<UserDto> Get(Guid id)
    {
        var user = await dbContext.Users.Select(
            x => new UserDto
            {
                Id = x.Id,
                Username = x.Username,
                IsAdmin = x.IsAdmin,
                AvatarImageId = x.AvatarImageId,
            }
        ).FirstAsync(x => x.Id == id);

        return user;
    }
}
