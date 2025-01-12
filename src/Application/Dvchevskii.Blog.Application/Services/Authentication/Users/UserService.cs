using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Users;
using Dvchevskii.Blog.Application.Mapping.Authentication.Users;
using Dvchevskii.Blog.Core.Entities.Authentication.Users;
using Dvchevskii.Blog.Core.Repositories.Authentication.Users;

namespace Dvchevskii.Blog.Application.Services.Authentication.Users;

internal class UserService(IUserRepository repository) : IUserService
{
    public Task<bool> ExistsByUsername(string username)
    {
        return repository.ExistsByUsername(username);
    }

    public async Task<UserDto?> FindByUsername(string username)
    {
        var user = await repository.FindByUsername(username);

        if (user == null)
        {
            return null;
        }

        return UserMapper.MapDto(user);
    }

    public async Task<UserDto> Create(CreateUserDto dto)
    {
        var user = User.Create(dto);

        await repository.Create(user);

        return UserMapper.MapDto(user);
    }

    public async Task<UserDto> Update(UpdateUserDto dto)
    {
        var user = await repository.GetById(dto.Id);

        user.Update(dto);
        await repository.Update(user);

        return UserMapper.MapDto(user);
    }
}
