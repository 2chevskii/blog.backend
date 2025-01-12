namespace Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Local;

public class UserSignInDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}
