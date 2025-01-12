using Dvchevskii.Blog.Core.Entities.Authentication.Users;

namespace Dvchevskii.Blog.Core.Repositories.Authentication.Users;

public interface IUserRepository
{
    Task<User> GetById(Guid id);
    Task<User?> FindByUsername(string username);
    Task<bool> ExistsByUsername(string username);

    Task<User> Create(User user);
    Task<User> Update(User user);
    Task Delete(User user);
}
