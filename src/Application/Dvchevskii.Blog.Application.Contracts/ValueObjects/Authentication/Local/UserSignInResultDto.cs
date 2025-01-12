using System.Diagnostics.CodeAnalysis;
using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;

namespace Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Local;

public class UserSignInResultDto
{
    [MemberNotNullWhen(true, nameof(User))]
    [MemberNotNullWhen(false, nameof(ErrorCode))]
    public required bool IsSuccess { get; init; }
    public string? ErrorCode { get; init; }
    public UserDto? User { get; init; }
}
