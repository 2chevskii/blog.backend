using Dvchevskii.Blog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Admin.Services;

internal class UserManager(BlogDbContext dbContext)
{
    public async Task UpdateAvatar(Guid userId, Guid? avatarImageId)
    {
        var user = await dbContext.Users.FirstAsync(x => x.Id == userId);
        user.AvatarImageId = avatarImageId;
        await dbContext.SaveChangesAsync();
    }
}
