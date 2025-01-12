using Dvchevskii.Blog.Core.Entities.Authentication.Accounts;
using Dvchevskii.Blog.Core.Repositories.Authentication.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Infrastructure.Repositories.Authentication.Passwords;

internal class PasswordAccountRepository(BlogDbContext dbContext) : IPasswordAccountRepository
{
    public Task<PasswordAccount> GetById(Guid id)
    {
        return dbContext.PasswordAccounts.FirstAsync(x => x.Id == id);
    }

    public Task<PasswordAccount?> FindByUserId(Guid userId)
    {
        return dbContext.PasswordAccounts.FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<PasswordAccount> Create(PasswordAccount passwordAccount)
    {
        dbContext.PasswordAccounts.Add(passwordAccount);
        await dbContext.SaveChangesAsync();
        return passwordAccount;
    }

    public async Task<PasswordAccount> Update(PasswordAccount passwordAccount)
    {
        dbContext.PasswordAccounts.Update(passwordAccount);
        await dbContext.SaveChangesAsync();
        return passwordAccount;
    }

    public async Task Delete(PasswordAccount passwordAccount)
    {
        dbContext.Remove(passwordAccount);
        await dbContext.SaveChangesAsync();
    }
}
