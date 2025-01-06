using System.Security.Claims;
using Dvchevskii.Blog.Auth.Models;
using Dvchevskii.Blog.Auth.Services;
using Dvchevskii.Blog.Shared.Authentication.Context;
using Dvchevskii.Blog.Shared.Contracts.Authentication.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.Auth.Controllers;

[ApiController]
[Route("[controller]")]
internal sealed class AuthController(LocalAuthService localAuthService, IAuthenticationScope authenticationScope)
    : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> LocalSignUp(LocalSignUpRequest request)
    {
        var result = await localAuthService.SignUp(request);
        return result.Type == LocalSignUpResultType.OK ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> LocalSignIn(LocalSignInRequest request)
    {
        var result = await localAuthService.SignIn(request);
        if (result.Type != LocalSignInResultType.OK)
        {
            return Unauthorized(result);
        }

        return SignIn(
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.NameIdentifier, result.UserInfo.Id.ToString()),
                        new Claim(ClaimTypes.GivenName, result.UserInfo.Username),
                        new Claim(ClaimTypes.Role, result.UserInfo.IsAdmin ? "admin" : "user"),
                    ],
                    CookieAuthenticationDefaults.AuthenticationScheme
                )
            )
        );
    }

    [HttpPost("signout")]
    public IActionResult ApplicationSignOut()
    {
        return SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet("user-info")]
    public IActionResult GetUserInfo()
    {
        if (!authenticationScope.IsAuthenticated)
        {
            return Unauthorized();
        }

        return Ok(authenticationScope);
    }
}
