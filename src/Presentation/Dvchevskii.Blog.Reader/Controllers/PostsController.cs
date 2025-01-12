using Dvchevskii.Blog.Reader.Models;
using Dvchevskii.Blog.Reader.Services;
using Dvchevskii.Blog.Shared.Contracts.Assets.Images;
using Dvchevskii.Blog.Shared.Contracts.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.Reader.Controllers;

[ApiController]
[Route("[controller]")]
internal class PostsController(PostService postService, IImageAssetService imageAssetService, UserService userService)
    : ControllerBase
{
    [HttpGet]
    public async Task<PaginationQueryResult<PostShowcaseModel>> GetPosts(int offset, int limit)
    {
        var publishedPosts = await postService.GetPublishedList(offset, limit);

        var headerImageUrls = await imageAssetService.GetPreSignedUrlList(
            publishedPosts.Items
                .Where(x => x.HeaderImageId.HasValue)
                .Select(x => x.HeaderImageId!.Value)
        );

        var result = publishedPosts.Map(post => new PostShowcaseModel
        {
            Id = post.Id,
            Slug = post.Slug,
            Title = post.Title,
            Tagline = post.Tagline,
            HeaderImageUrl = post.HeaderImageId.HasValue ? headerImageUrls[post.HeaderImageId.Value] : null,
        });

        return result;
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<PostReadModel>> GetBySlug(string slug)
    {
        var post = await postService.FindBySlug(slug);

        if (post == null)
        {
            return NotFound();
        }

        Uri? headerImageUrl = null;

        if (post.HeaderImageId.HasValue)
        {
            headerImageUrl = await imageAssetService.GetPreSignedUrl(post.HeaderImageId.Value);
        }

        var lastModifiedTimestamp = post.UpdatedAt ?? post.CreatedAt;
        var lastModifiedById = post.UpdatedBy ?? post.CreatedBy;

        var lastModifiedUser = await userService.Get(lastModifiedById);

        var lastModifiedAvatarUrl = lastModifiedUser.AvatarImageId.HasValue
            ? await imageAssetService.GetPreSignedUrl(lastModifiedUser.AvatarImageId.Value)
            : null;

        var readModel = new PostReadModel
        {
            Id = post.Id,
            Slug = post.Slug,
            Title = post.Title,
            Tagline = post.Tagline,
            Body = post.Body,
            HeaderImageUrl = headerImageUrl,
            LastModifiedBy = new PostAuthorModel
            {
                Id = lastModifiedById,
                Timestamp = lastModifiedTimestamp,
                Username = lastModifiedUser.Username,
                AvatarUrl = lastModifiedAvatarUrl
            },
        };

        return readModel;
    }
}
