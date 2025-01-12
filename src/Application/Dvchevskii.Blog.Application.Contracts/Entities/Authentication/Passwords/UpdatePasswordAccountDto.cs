using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;

namespace Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Passwords;

public class UpdatePasswordAccountDto
{
    public required Guid Id { get; init; }
    public required UserDto User { get; init; }
    public required string Value { get; init; }
    public byte[] HashedValue { get; set; }
}
