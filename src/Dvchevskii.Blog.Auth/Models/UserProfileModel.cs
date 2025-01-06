namespace Dvchevskii.Blog.Auth.Models;

public class UserProfileModel
{
    public required Guid Id { get; init; }
    public required string? Username { get; init; }
    public required bool IsAdmin { get; init; }
    public required Guid? AvatarImageId { get; init; }
    public required Uri? AvatarUrl { get; set; }
    public required DateTime JoinDate { get; init; }
}
