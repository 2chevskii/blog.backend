using Dvchevskii.Blog.Auth.Models;
using Dvchevskii.Blog.Entities.Authentication.Accounts;
using Dvchevskii.Blog.Entities.Authentication.Users;
using Dvchevskii.Blog.Infrastructure;
using Dvchevskii.Blog.Shared.Authentication.Context;
using Dvchevskii.Blog.Shared.Authentication.Passwords;
using Dvchevskii.Blog.Shared.Contracts.Authentication.Context;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Auth.Services;

internal sealed class LocalAuthService(
    BlogDbContext dbContext,
    PasswordHasher passwordHasher,
    IAuthenticationContextProvider authenticationContextProvider)
{
    public async Task<LocalSignUpResult> SignUp(LocalSignUpRequest request)
    {
        using var authScope = authenticationContextProvider.CreateSystemScope();

        var existingUser = await FindByUsernameLogin(request.Username, false);

        if (existingUser != null)
        {
            return new LocalSignUpResult(LocalSignUpResultType.USERNAME_EXISTS);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            IsAdmin = false,
            Username = request.Username,
            IsBanned = false,
            PasswordAccount = new PasswordAccount
            {
                Id = Guid.NewGuid(),
                PasswordHash = passwordHasher.Hash(request.Password),
                IsDeactivated = false,
                UserId = default(Guid),
            },
        };

        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync();

        return new LocalSignUpResult(LocalSignUpResultType.OK);
    }

    public async Task<LocalSignInResult> SignIn(LocalSignInRequest request)
    {
        if (IsEmailLogin(request.Login))
        {
            throw new NotImplementedException("Email login is not implemented");
        }

        var user = await FindByUsernameLogin(request.Login, true);

        return user switch
        {
            null => new LocalSignInResult(LocalSignInResultType.USER_NOT_FOUND),
            { IsBanned: true } => new LocalSignInResult(LocalSignInResultType.USER_BANNED),
            { PasswordAccount: null } => new LocalSignInResult(LocalSignInResultType.PASSWORD_ACCOUNT_NOT_FOUND),
            { PasswordAccount.IsDeactivated: true } => new LocalSignInResult(
                LocalSignInResultType.PASSWORD_ACCOUNT_DEACTIVATED
            ),
            not null when !passwordHasher.Verify(request.Password, user.PasswordAccount.PasswordHash) =>
                new LocalSignInResult(LocalSignInResultType.PASSWORD_INVALID),
            var _ => new LocalSignInResult(
                LocalSignInResultType.OK,
                new LocalUserInfo(user.Id, user.Username, user.IsAdmin)
            ),
        };
    }

    private static bool IsEmailLogin(string login) => login.Contains('@');

    private async Task<User?> FindByUsernameLogin(string login, bool loadPasswordAccount)
    {
        var query = dbContext.Users.AsQueryable()
            .Where(x => x.Username != null && x.Username == login);

        if (loadPasswordAccount)
        {
            query = query.Include(x => x.PasswordAccount)
                .Where(x => x.PasswordAccount != null && !x.PasswordAccount.IsDeactivated);
        }

        var user = await query.FirstOrDefaultAsync();
        return user;
    }
}
