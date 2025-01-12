using System.Text;
using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Passwords;
using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Passwords;
using Dvchevskii.Blog.Application.Mapping.Authentication.Passwords;
using Dvchevskii.Blog.Core.Entities.Authentication.Accounts;
using Dvchevskii.Blog.Core.Repositories.Authentication.Accounts;
using Microsoft.AspNetCore.Identity;

namespace Dvchevskii.Blog.Application.Services.Authentication.Passwords;

internal class PasswordAccountService(
    IPasswordAccountRepository repository,
    PasswordHashService passwordHashService
) : IPasswordAccountService
{
    public async Task<PasswordAccountDto?> FindForUser(Guid userId)
    {
        var passwordAccount = await repository.FindByUserId(userId);
        if (passwordAccount == null)
        {
            return null;
        }

        return PasswordAccountMapper.MapDto(passwordAccount);
    }

    public async Task<PasswordAccountDto> Create(CreatePasswordAccountDto dto)
    {
        dto.HashedValue = passwordHashService.Hash(dto.Value);

        var passwordAccount = PasswordAccount.Create(dto);
        await repository.Create(passwordAccount);

        return PasswordAccountMapper.MapDto(passwordAccount);
    }

    public async Task<PasswordAccountDto> Deactivate(Guid id)
    {
        var passwordAccount = await repository.GetById(id);
        passwordAccount.SetDeactivated();
        await repository.Update(passwordAccount);

        return PasswordAccountMapper.MapDto(passwordAccount);
    }

    public async Task<PasswordAccountDto> UpdateHash(UpdatePasswordAccountDto dto)
    {
        var passwordAccount = await repository.GetById(dto.Id);
        dto.HashedValue = passwordHashService.Hash(dto.Value);
        passwordAccount.Update(dto);

        return PasswordAccountMapper.MapDto(passwordAccount);
    }

    public async Task<bool> Verify(VerifyPasswordHashDto dto)
    {
        var passwordAccount = await repository.GetById(dto.Id);
        var result = passwordHashService.Verify(dto.Value, passwordAccount.PasswordHash);

        return result;
    }
}
