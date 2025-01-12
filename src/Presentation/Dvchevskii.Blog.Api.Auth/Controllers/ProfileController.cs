using Dvchevskii.Blog.Api.Auth.Models;
using Dvchevskii.Blog.Api.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.Api.Auth.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController(UserProfileService userProfileService) : ControllerBase
{
    [HttpGet]
    public async Task<UserProfileModel> Get()
    {
        var profile = await userProfileService.GetProfile();
        return profile;
    }

    [HttpPatch("avatar")]
    public async Task<UserProfileModel> UpdateAvatar(UpdateAvatarRequest request)
    {
        await userProfileService.UpdateAvatar(request.ImageId);
        var profile = await userProfileService.GetProfile();
        return profile;
    }

    [HttpDelete("avatar")]
    public async Task<UserProfileModel> DeleteAvatar()
    {
        await userProfileService.UpdateAvatar(null);
        var profile = await userProfileService.GetProfile();
        return profile;
    }
}
