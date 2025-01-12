using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Local;

namespace Dvchevskii.Blog.Application.Contracts.Services.Authentication;

public interface IBlogAuthenticationService
{
    Task<UserSignUpResultDto> SignUp(UserSignUpDto userSignUpDto);
    Task<UserSignInResultDto> SignIn(UserSignInDto userSignInDto);
}
