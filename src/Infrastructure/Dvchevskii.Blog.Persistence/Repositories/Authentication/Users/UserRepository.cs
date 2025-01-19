using Dvchevskii.Blog.Core.Entities.Authentication.Users;
using Dvchevskii.Blog.Core.Repositories.Authentication.Users;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Infrastructure.Repositories.Authentication.Users;

internal class UserRepository(BlogDbContext dbContext) : IUserRepository
{
    public Task<User> GetById(Guid id)
    {
        return dbContext.Users.FirstAsync(x => x.Id == id);
    }

    public Task<User?> FindByUsername(string username)
    {
        return dbContext.Users.FirstOrDefaultAsync(
            x => x.Username != null
                 && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
        );
    }

    public Task<bool> ExistsByUsername(string username)
    {
        return dbContext.Users.AnyAsync(
            x => x.Username != null
                 && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
        );
    }

    public async Task<User> Create(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User> Update(User user)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public async Task Delete(User user)
    {
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Guid?> GetAvatarImageId(Guid userId)
    {
        var imageId = await dbContext.Users.Where(x => x.Id == userId)
            .Select(x => x.AvatarImageId)
            .FirstOrDefaultAsync();

        return imageId;
    }

    public async Task<List<User>> GetList(IEnumerable<Guid> ids)
    {
        var users = await dbContext.Users.Where(user => ids.Contains(user.Id))
            .ToListAsync();

        return users;
    }

    public Task<Dictionary<Guid, Guid?>> GetAvatarImageIdList(IEnumerable<Guid> userIds)
    {
        return dbContext.Users
            .Where(user => userIds.Contains(user.Id))
            .ToDictionaryAsync(k => k.Id, v => v.AvatarImageId);
    }
}
