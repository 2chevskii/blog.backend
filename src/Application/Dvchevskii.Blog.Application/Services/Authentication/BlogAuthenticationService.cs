using Dvchevskii.Blog.Application.Contracts.Services.Authentication;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Local;
using Dvchevskii.Blog.Application.Services.Authentication.Local;

namespace Dvchevskii.Blog.Application.Services.Authentication;

internal class BlogAuthenticationService(
    LocalAuthenticationService localAuthenticationService
) : IBlogAuthenticationService
{
    public async Task<UserSignUpResultDto> SignUp(UserSignUpDto userSignUpDto)
    {
        var result = await localAuthenticationService.SignUp(userSignUpDto);

        return result;
    }

    public async Task<UserSignInResultDto> SignIn(UserSignInDto userSignInDto)
    {
        var result = await localAuthenticationService.SignIn(new UserSignInDto
        {
            Username = userSignInDto.Username,
            Password = userSignInDto.Password,
        });

        return result;
    }
}
