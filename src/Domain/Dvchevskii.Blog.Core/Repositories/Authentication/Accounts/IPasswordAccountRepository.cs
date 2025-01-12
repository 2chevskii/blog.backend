using Dvchevskii.Blog.Core.Entities.Authentication.Accounts;

namespace Dvchevskii.Blog.Core.Repositories.Authentication.Accounts;

public interface IPasswordAccountRepository
{
    Task<PasswordAccount> GetById(Guid id);
    Task<PasswordAccount?> FindByUserId(Guid userId);

    Task<PasswordAccount> Create(PasswordAccount passwordAccount);
    Task<PasswordAccount> Update(PasswordAccount passwordAccount);
    Task Delete(PasswordAccount passwordAccount);
}
