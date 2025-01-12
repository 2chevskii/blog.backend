using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;

namespace Dvchevskii.Blog.Application.Contracts.Services.Authentication.Users;

public interface IUserService
{
    Task<bool> ExistsByUsername(string username);
    Task<UserDto> Create(CreateUserDto dto);
    Task<UserDto> Update(UpdateUserDto dto);
    Task<UserDto?> FindByUsername(string username);
}
