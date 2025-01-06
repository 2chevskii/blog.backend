using Dvchevskii.Blog.Auth.Models;
using Dvchevskii.Blog.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.Auth.Controllers;

[ApiController]
[Route("[controller]")]
internal class ProfileController(UserProfileService userProfileService) : ControllerBase
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
