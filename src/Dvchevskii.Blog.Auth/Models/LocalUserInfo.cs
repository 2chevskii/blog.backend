namespace Dvchevskii.Blog.Auth.Models;

public sealed record LocalUserInfo(Guid Id, string? Username, bool IsAdmin);