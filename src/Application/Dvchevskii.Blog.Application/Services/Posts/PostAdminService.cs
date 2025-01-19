using Dvchevskii.Blog.Application.Contracts.Entities.Posts;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.Services.Posts;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;

namespace Dvchevskii.Blog.Application.Services.Posts;

internal class PostAdminService(
    IPostService postService,
    IUserService userService,
    IUserAvatarService userAvatarService
) : IPostAdminService
{
    public async Task<LimitedQueryResult<PostInfoDto>> GetInfoList(LimitedQuerySettings settings, bool onlyPublished)
    {
        var posts = await postService.GetList(settings, onlyPublished);

        var infoList = await posts.MapAsync(async post =>
        {
            PostEditorDto? updatedBy = null;

            var creator = await userService.Get(post.CreatedBy);

            var createdBy = new PostEditorDto
            {
                Id = post.CreatedBy,
                Timestamp = post.CreatedAt,
                Username = creator.Username,
                AvatarUrl = await userAvatarService.GetAvatarUrl(creator.Id),
            };

            if (post.UpdatedBy.HasValue)
            {
                var updater = await userService.Get(post.UpdatedBy.Value);

                updatedBy = new PostEditorDto
                {
                    Id = post.UpdatedBy.Value,
                    Timestamp = post.UpdatedAt!.Value,
                    Username = updater.Username,
                    AvatarUrl = await userAvatarService.GetAvatarUrl(updater.Id),
                };
            }

            var info = new PostInfoDto
            {
                Id = post.Id,
                Slug = post.Slug,
                Title = post.Title,
                IsPublished = post.IsPublished,
                CreatedBy = createdBy,
                UpdatedBy = updatedBy,
            };

            return info;
        });

        return infoList;
    }
}
