using Dvchevskii.Blog.Api.Auth.Models;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Authentication.Context;
using Dvchevskii.Blog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Api.Auth.Services;

public class UserProfileService(
    BlogDbContext dbContext,
    IAuthenticationScope authenticationScope
)
{
    public async Task UpdateAvatar(Guid? avatarImageId)
    {
        if (!authenticationScope.IsAuthenticated)
        {
            throw new Exception("Not authenticated");
        }

        var user = await dbContext.Users.FirstAsync(x => x.Id == authenticationScope.UserId);
        user.AvatarImageId = avatarImageId;
        await dbContext.SaveChangesAsync();
    }

    public async Task<UserProfileModel> GetProfile()
    {
        if (!authenticationScope.IsAuthenticated)
        {
            throw new Exception("not authenticated");
        }

        var profileModel = await dbContext.Users.Select(x => new UserProfileModel
        {
            Id = x.Id,
            Username = x.Username,
            IsAdmin = x.IsAdmin,
            JoinDate = x.AuditInfo.CreatedAt,
            AvatarImageId = x.AvatarImageId,
            AvatarUrl = null,
        }).FirstAsync(x => x.Id == authenticationScope.UserId);

        /*if (profileModel.AvatarImageId.HasValue)
        {
            var avatarUrl = await imageAssetService.GetPreSignedUrl(profileModel.AvatarImageId.Value);
            profileModel.AvatarUrl = avatarUrl;
        }*/

        return profileModel;
    }
}
