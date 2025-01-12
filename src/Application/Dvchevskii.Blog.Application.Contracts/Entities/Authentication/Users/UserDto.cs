namespace Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;

public class UserDto
{
    public required Guid Id { get; init; }
    public required string? Username { get; init; }
    public required Guid? AvatarImageId { get; init; }

    public required bool IsAdmin { get; init; }
    public required bool IsBanned { get; init; }

    public required DateTime CreatedAt { get; init; }
    public required DateTime? UpdatedAt { get; init; }
}
