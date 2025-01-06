namespace Dvchevskii.Blog.Shared.Contracts.Authentication;

public readonly struct AuthenticationData
{
    public required Guid UserId { get; init; }
    public required bool IsAdmin { get; init; }
    public string? Username { get; init; }
}
