using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Passwords;
using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Context;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Passwords;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Local;
using Dvchevskii.Blog.Application.Extensions.Authentication.Context;
using Microsoft.Extensions.Logging;

namespace Dvchevskii.Blog.Application.Services.Authentication.Local;

internal class LocalAuthenticationService(
    ILogger<LocalAuthenticationService> logger,
    IUserService userService,
    IPasswordAccountService passwordAccountService,
    IAuthenticationContextProvider authenticationContextProvider
)
{
    public async Task<UserSignUpResultDto> SignUp(UserSignUpDto dto)
    {
        using var _ = authenticationContextProvider.CreateSystemScope();

        if (!IsValidUsername(dto.Username))
        {
            return new UserSignUpResultDto
            {
                IsSuccess = false,
                ErrorCode = "USERNAME_INVALID"
            };
        }

        if (!IsValidPassword(dto.Password))
        {
            return new UserSignUpResultDto
            {
                IsSuccess = false,
                ErrorCode = "PASSWORD_INVALID"
            };
        }

        if (!await IsAvailableUsername(dto.Username))
        {
            return new UserSignUpResultDto
            {
                IsSuccess = false,
                ErrorCode = "USERNAME_EXISTS"
            };
        }

        var createUserDto = new CreateUserDto
        {
            Username = dto.Username,
            IsAdmin = false,
            IsBanned = false,
            AvatarImageId = null,
        };

        var user = await userService.Create(createUserDto);

        var createPasswordAccountDto = new CreatePasswordAccountDto
        {
            User = user,
            Value = dto.Password,
        };

        var passwordAccount = await passwordAccountService.Create(createPasswordAccountDto);

        logger.LogInformation(
            "Created user {UserId} with username {Username} and password account {PasswordAccountId}",
            user.Id,
            user.Username,
            passwordAccount.Id
        );

        return new UserSignUpResultDto
        {
            IsSuccess = true,
            UserId = user.Id,
        };
    }

    public async Task<UserSignInResultDto> SignIn(UserSignInDto dto)
    {
        var user = await userService.FindByUsername(dto.Username);

        if (user == null)
        {
            return new UserSignInResultDto
            {
                IsSuccess = false,
                ErrorCode = "USER_NOT_FOUND",
            };
        }

        var passwordAccount = await passwordAccountService.FindForUser(user.Id);

        if (passwordAccount == null)
        {
            return new UserSignInResultDto
            {
                IsSuccess = false,
                ErrorCode = "PASSWORD_ACCOUNT_NOT_FOUND",
            };
        }

        if (passwordAccount.IsDeactivated)
        {
            return new UserSignInResultDto
            {
                IsSuccess = false,
                ErrorCode = "PASSWORD_ACCOUNT_DEACTIVATED",
            };
        }

        if (
            !await passwordAccountService.Verify(
                new VerifyPasswordHashDto
                {
                    Id = passwordAccount.Id,
                    User = user,
                    Value = dto.Password,
                }
            )
        )
        {
            return new UserSignInResultDto
            {
                IsSuccess = false,
                ErrorCode = "PASSWORD_INVALID",
            };
        }

        return new UserSignInResultDto
        {
            IsSuccess = true,
            User = user,
        };
    }

    private async Task<bool> IsAvailableUsername(string username)
    {
        var exists = await userService.ExistsByUsername(username);
        return !exists;
    }

    private bool IsValidUsername(string username)
    {
        return !string.IsNullOrWhiteSpace(username);
    }

    private bool IsValidPassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password);
    }
}
