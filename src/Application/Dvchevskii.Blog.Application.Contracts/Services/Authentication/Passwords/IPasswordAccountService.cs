using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Passwords;

namespace Dvchevskii.Blog.Application.Contracts.Services.Authentication.Passwords;

public interface IPasswordAccountService
{
    Task<PasswordAccountDto?> FindForUser(Guid userId);
    Task<PasswordAccountDto> Create(CreatePasswordAccountDto dto);
    Task<PasswordAccountDto> Deactivate(Guid id);
    Task<PasswordAccountDto> UpdateHash(UpdatePasswordAccountDto dto);
    Task<bool> Verify(VerifyPasswordHashDto dto);
}
