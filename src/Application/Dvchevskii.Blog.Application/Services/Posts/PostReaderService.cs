using Dvchevskii.Blog.Application.Contracts.Entities.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.Entities.Posts;
using Dvchevskii.Blog.Application.Contracts.Services.Authentication.Users;
using Dvchevskii.Blog.Application.Contracts.Services.Files.Images;
using Dvchevskii.Blog.Application.Contracts.Services.Posts;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;

namespace Dvchevskii.Blog.Application.Services.Posts;

internal class PostReaderService(
    IPostService postService,
    IUserService userService,
    IUserAvatarService userAvatarService,
    IImageServiceClient imageServiceClient
) : IPostReaderService
{
    public async Task<LimitedQueryResult<PostFeedEntryDto>> GetFeed(LimitedQuerySettings settings)
    {
        var posts = await postService.GetList(settings, onlyPublished: true);
        var authorIds = posts.Items
            .Select(post => post.UpdatedBy ?? post.CreatedBy)
            .Distinct()
            .ToArray();
        var authors = await userService.GetList(authorIds);
        var authorAvatarUrls = await userAvatarService.GetAvatarUrls(authorIds);
        var headerImageUrls = await imageServiceClient.GetUrls(posts.Items
            .Where(post => post.HeaderImageId.HasValue)
            .Select(post => post.HeaderImageId!.Value)
        );

        return posts.Map(post =>
        {
            UserDto author;

            if (post.UpdatedBy.HasValue)
            {
                author = authors.Find(x => x.Id == post.UpdatedBy.Value)!;
            }
            else
            {
                author = authors.Find(x => x.Id == post.CreatedBy)!;
            }

            return new PostFeedEntryDto
            {
                Id = post.Id,
                Slug = post.Slug,
                Title = post.Title,
                HeaderImageUrl = post.HeaderImageId.HasValue ? headerImageUrls[post.HeaderImageId.Value] : null,
                LastModifiedBy = new PostEditorDto
                {
                    Id = author.Id,
                    Timestamp = post.UpdatedAt ?? post.CreatedAt,
                    Username = author.Username,
                    AvatarUrl = author.AvatarImageId.HasValue ? authorAvatarUrls[author.Id] : null,
                },
                BodyPreview = post.Body.Substring(0, 300),
            };
        });
    }

    public async Task<PostReadDto> Get(string slug)
    {
        var post = await postService.Get(slug, true);
        var author = await userService.Get(post.UpdatedBy ?? post.CreatedBy);
        var authorAvatarUrl = await userAvatarService.GetAvatarUrl(author.Id);
        Uri? headerImageUrl = null;
        if (post.HeaderImageId.HasValue)
        {
            headerImageUrl = await imageServiceClient.GetUrl(post.HeaderImageId.Value);
        }

        return new PostReadDto
        {
            Id = post.Id,
            Slug = post.Slug,
            Title = post.Title,
            Body = post.Body,
            HeaderImageUrl = headerImageUrl,
            LastModifiedBy = new PostEditorDto
            {
                Id = author.Id,
                Username = author.Username,
                Timestamp = post.UpdatedAt ?? post.CreatedAt,
                AvatarUrl = authorAvatarUrl,
            }
        };
    }
}
