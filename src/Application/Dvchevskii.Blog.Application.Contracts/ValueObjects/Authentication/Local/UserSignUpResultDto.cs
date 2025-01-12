namespace Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Local;

public class UserSignUpResultDto
{
    public required bool IsSuccess { get; init; }
    public Guid? UserId { get; init; }
    public string? ErrorCode { get; init; }
}
