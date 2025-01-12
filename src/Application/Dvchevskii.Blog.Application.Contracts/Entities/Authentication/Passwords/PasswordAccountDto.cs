namespace Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Passwords;

public class PasswordAccountDto
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required bool IsDeactivated { get; init; }
}
