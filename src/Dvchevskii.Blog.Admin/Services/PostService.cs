using Dvchevskii.Blog.Admin.Models;
using Dvchevskii.Blog.Entities.Posts;
using Dvchevskii.Blog.Infrastructure;
using Dvchevskii.Blog.Shared.Contracts.Assets.Images;
using Dvchevskii.Blog.Shared.Contracts.Authentication.Context;
using Dvchevskii.Blog.Shared.Contracts.Pagination;
using Dvchevskii.Blog.Shared.Pagination;
using Dvchevskii.Blog.Shared.Posts;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Admin.Services;

internal class PostService(
    BlogDbContext dbContext,
    IAuthenticationScope authenticationScope,
    Sluggifier sluggifier,
    IImageAssetService imageAssetService
)
{
    public async Task<PaginationQueryResult<PostInfoModel>> GetInfoList(int offset, int limit, bool includeNonPublished)
    {
        if (!authenticationScope.IsAdmin)
        {
            throw new Exception("not an admin");
        }

        var query = dbContext.Posts.AsQueryable();

        if (!includeNonPublished)
        {
            query = query.Where(x => x.IsPublished);
        }

        var paginationResult = await query
            .Select(post => new PostInfoModel
            {
                Id = post.Id,
                Slug = post.Slug,
                Title = post.Title,
                IsPublished = post.IsPublished,
                CreatedBy = new PostAuthorInfo
                {
                    Id = post.AuditInfo.CreatedBy,
                    Timestamp = post.AuditInfo.CreatedAt,
                    Username = dbContext.Users.First(x => x.Id == post.AuditInfo.CreatedBy).Username,
                },
                UpdatedBy = post.AuditInfo.UpdatedAt.HasValue && post.AuditInfo.UpdatedBy.HasValue
                    ? new PostAuthorInfo
                    {
                        Id = post.AuditInfo.UpdatedBy.Value,
                        Timestamp = post.AuditInfo.UpdatedAt.Value,
                        Username = dbContext.Users.First(x => x.Id == post.AuditInfo.UpdatedBy.Value).Username
                    }
                    : null,
            }).ToPaginatedAsync(offset, limit);

        return paginationResult;
    }

    public async Task<PostEditModel> Create(CreatePostRequest request)
    {
        if (!authenticationScope.IsAdmin)
        {
            throw new Exception("Not admin");
        }

        var slug = sluggifier.CreateSlug(request.Title);
        var post = new Post
        {
            Id = Guid.NewGuid(),
            Slug = slug,
            Title = request.Title,
            Tagline = request.Tagline,
            Body = request.Body,
            IsPublished = request.IsPublished,
            HeaderImageId = request.HeaderImageId,
        };

        dbContext.Add(post);
        await dbContext.SaveChangesAsync();

        var creator = await dbContext.Users.FirstAsync(x => x.Id == post.AuditInfo.CreatedBy);

        var headerImageUrl = post.HeaderImageId.HasValue
            ? await imageAssetService.GetPreSignedUrl(post.HeaderImageId.Value)
            : null;

        return new PostEditModel(
            post.Id,
            post.Slug,
            post.Title,
            post.Tagline,
            post.Body,
            post.IsPublished,
            new PostAuthorInfo
            {
                Id = creator.Id,
                Username = creator.Username,
                Timestamp = post.AuditInfo.CreatedAt,
            },
            null,
            post.HeaderImageId,
            headerImageUrl
        );
    }

    public async Task<PostEditModel> Update(UpdatePostRequest request)
    {
        if (!authenticationScope.IsAdmin)
        {
            throw new Exception("not admin");
        }

        var post = await dbContext.Posts.FirstAsync(x => x.Id == request.Id);

        if (post.Title != request.Title)
        {
            var slug = sluggifier.CreateSlug(request.Title);
            post.Title = request.Title;
            post.Slug = slug;
        }

        post.Tagline = request.Tagline;
        post.Body = request.Body;
        post.IsPublished = request.IsPublished;
        post.HeaderImageId = request.HeaderImageId;

        await dbContext.SaveChangesAsync();

        var creator = await dbContext.Users.FirstAsync(x => x.Id == post.AuditInfo.CreatedBy);
        var updater = await dbContext.Users.FirstAsync(x => x.Id == post.AuditInfo.UpdatedBy!.Value);

        var headerImageUrl = post.HeaderImageId.HasValue
            ? await imageAssetService.GetPreSignedUrl(post.HeaderImageId.Value)
            : null;

        return new PostEditModel(
            post.Id,
            post.Slug,
            post.Title,
            post.Tagline,
            post.Body,
            post.IsPublished,
            new PostAuthorInfo
            {
                Id = creator.Id,
                Username = creator.Username,
                Timestamp = post.AuditInfo.CreatedAt,
            },
            new PostAuthorInfo
            {
                Id = updater.Id,
                Username = updater.Username,
                Timestamp = post.AuditInfo.UpdatedAt!.Value,
            },
            post.HeaderImageId,
            headerImageUrl
        );
    }
}
