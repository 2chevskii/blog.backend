namespace Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Local;

public class UserSignUpDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}
