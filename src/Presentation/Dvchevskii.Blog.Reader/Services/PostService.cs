using Dvchevskii.Blog.Infrastructure;
using Dvchevskii.Blog.Reader.DTOs;
using Dvchevskii.Blog.Reader.Extensions;
using Dvchevskii.Blog.Reader.Models;
using Dvchevskii.Blog.Shared.Contracts.Assets.Images;
using Dvchevskii.Blog.Shared.Contracts.Pagination;
using Dvchevskii.Blog.Shared.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Reader.Services;

internal class PostService(BlogDbContext dbContext, IImageAssetService imageAssetService)
{
    public async Task<PaginationQueryResult<PostDto>> GetPublishedList(int offset, int limit)
    {
        var posts = await dbContext.Posts
            .Where(post => post.IsPublished)
            .Select(post => post.ToDto())
            .ToPaginatedAsync(offset, limit);

        return posts;
    }

    public async Task<PostDto?> FindBySlug(string slug)
    {
        var post = await dbContext.Posts
            .Where(post => post.IsPublished && post.Slug == slug)
            .Select(post => post.ToDto())
            .FirstOrDefaultAsync();

        return post;
    }
}
