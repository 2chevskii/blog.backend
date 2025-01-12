namespace Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;

public class UpdateUserDto
{
    public required Guid Id { get; set; }
    public required string? Username { get; init; }
    public required bool IsAdmin { get; init; }
    public required bool IsBanned { get; init; }
    public required Guid? AvatarImageId { get; init; }
}
