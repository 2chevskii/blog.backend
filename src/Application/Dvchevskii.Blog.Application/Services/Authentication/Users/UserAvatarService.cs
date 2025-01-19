using System.Runtime.CompilerServices;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.Services.Files.Images;
using Dvchevskii.Blog.Core.Repositories.Authentication.Users;

namespace Dvchevskii.Blog.Application.Services.Authentication.Users;

internal class UserAvatarService(
    IImageServiceClient imageServiceClient,
    IUserRepository userRepository
) : IUserAvatarService
{
    public async Task<Uri?> GetAvatarUrl(Guid userId)
    {
        var avatarImageId = await userRepository.GetAvatarImageId(userId);
        if (avatarImageId == null)
        {
            return null;
        }

        var imageUrl = await imageServiceClient.GetUrl(avatarImageId.Value);
        return imageUrl;
    }

    public async Task<Dictionary<Guid, Uri?>> GetAvatarUrls(IEnumerable<Guid> userIds)
    {
        var avatarIds = await userRepository.GetAvatarImageIdList(userIds);
        var urls = await imageServiceClient.GetUrls(avatarIds.Values.Where(x => x.HasValue).Select(x => x!.Value));

        return avatarIds.ToDictionary(
            k => k.Key,
            v => v.Value.HasValue ? urls[v.Value.Value] : null
        );
    }
}
