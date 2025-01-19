namespace Dvchevskii.Blog.Application.Contracts.Services.Authentication.Users;

public interface IUserAvatarService
{
    Task<Uri?> GetAvatarUrl(Guid userId);
    Task<Dictionary<Guid, Uri?>> GetAvatarUrls(IEnumerable<Guid> userIds);
}
