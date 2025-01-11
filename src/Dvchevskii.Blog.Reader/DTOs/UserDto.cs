namespace Dvchevskii.Blog.Reader.DTOs;

public class UserDto
{
    public required Guid Id { get; init; }
    public required bool IsAdmin { get; init; }
    public required string? Username { get; init; }
    public required Guid? AvatarImageId { get; init; }
}
