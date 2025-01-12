using System.Security.Claims;
using Dvchevskii.Blog.Api.Auth.Models;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Context;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Local;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.Api.Auth.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthController(
    IBlogAuthenticationService blogAuthenticationService,
    IAuthenticationScope authenticationScope
) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> LocalSignUp(LocalSignUpRequest request)
    {
        var result = await blogAuthenticationService.SignUp(
            new UserSignUpDto
            {
                Username = request.Username,
                Password = request.Password,
            }
        );

        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> LocalSignIn(LocalSignInRequest request)
    {
        var result = await blogAuthenticationService.SignIn(new UserSignInDto
        {
            Username = request.Login,
            Password = request.Password,
        });

        if (!result.IsSuccess)
        {
            return Unauthorized(result);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, result.User.Id.ToString()),
            new Claim(ClaimTypes.Role, result.User.IsAdmin ? "admin" : "user"),
        };

        if (result.User.Username != null)
        {
            claims.Add(new Claim(ClaimTypes.GivenName, result.User.Username));
        }

        return SignIn(
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    claims,
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
