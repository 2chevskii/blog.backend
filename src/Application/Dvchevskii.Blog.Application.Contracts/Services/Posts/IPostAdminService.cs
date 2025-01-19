using Dvchevskii.Blog.Application.Contracts.Entities.Posts;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;

namespace Dvchevskii.Blog.Application.Contracts.Services.Posts;

public interface IPostAdminService
{
    Task<LimitedQueryResult<PostInfoDto>> GetInfoList(LimitedQuerySettings settings, bool onlyPublished);
}
