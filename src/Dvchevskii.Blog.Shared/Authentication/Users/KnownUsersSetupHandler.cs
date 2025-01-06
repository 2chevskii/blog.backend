using System.Reflection;
using Dvchevskii.Blog.Entities.Authentication.Accounts;
using Dvchevskii.Blog.Entities.Authentication.Users;
using Dvchevskii.Blog.Infrastructure;
using Dvchevskii.Blog.Shared.Authentication.Passwords;
using Dvchevskii.Blog.Shared.Contracts.Authentication.Users;
using Dvchevskii.Blog.Shared.Contracts.Setup;
using Dvchevskii.Blog.Shared.Setup;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Shared.Authentication.Users;

[SetupHandler(Order = 0, SetupUser = nameof(KnownUsers.System))]
internal class KnownUsersSetupHandler(
    BlogDbContext dbContext,
    PasswordHasher passwordHasher
) : ISetupHandler
{
    public async Task ExecuteAsync()
    {
        var knownUserInfoFields = typeof(KnownUsers).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(f => f.FieldType == typeof(KnownUsers.KnownUserInfo))
            .ToArray();

        foreach (var knownUserInfoField in knownUserInfoFields)
        {
            var knownUserInfo = (KnownUsers.KnownUserInfo)knownUserInfoField.GetValue(null)!;

            var password = knownUserInfoField.GetCustomAttribute<KnownUsers.KnownUserPasswordAttribute>()?.Password;

            if (await dbContext.Users.AnyAsync(x => x.Id == knownUserInfo.Id))
            {
                continue;
            }

            var user = new User
            {
                Id = knownUserInfo.Id,
                IsAdmin = knownUserInfo.IsAdmin,
                Username = knownUserInfo.Username,
                PasswordAccount = password != null
                    ? new PasswordAccount
                    {
                        Id = Guid.NewGuid(),
                        UserId = knownUserInfo.Id,
                        PasswordHash = passwordHasher.Hash(password),
                        IsDeactivated = false,
                    }
                    : null,
                IsBanned = false,
                AvatarImageId = null,
            };
            dbContext.Add(user);
        }

        await dbContext.SaveChangesAsync();
    }
}
