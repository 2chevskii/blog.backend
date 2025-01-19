using System.Linq.Expressions;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;
using Dvchevskii.Blog.Application.Extensions.Pagination;
using Dvchevskii.Blog.Core.Entities.Posts;
using Dvchevskii.Blog.Core.Repositories.Posts;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Infrastructure.Repositories.Posts;

internal class PostRepository(BlogDbContext dbContext) : IPostRepository
{
    public async Task<List<Post>> GetList(bool onlyPublished = true)
    {
        var posts = await Query(onlyPublished)
            .OrderByDescending(post => post.AuditInfo.CreatedAt)
            .ToListAsync();
        return posts;
    }

    public async Task<LimitedQueryResult<Post>> GetList(LimitedQuerySettings settings, bool onlyPublished)
    {
        var posts = await Query(onlyPublished)
            .OrderByDescending(post => post.AuditInfo.CreatedAt)
            .ToLimitedResult(settings);
        return posts;
    }

    public async Task<LimitedQueryResult<T>> GetList<T>(
        LimitedQuerySettings settings,
        bool onlyPublished,
        Expression<Func<Post, T>> projectionExpression
    )
    {
        var posts = await Query(onlyPublished)
            .OrderByDescending(post => post.AuditInfo.CreatedAt)
            .Select(projectionExpression)
            .ToLimitedResult(settings);
        return posts;
    }

    public async Task<Post?> FindBySlug(string slug, bool onlyPublished = true)
    {
        var query = Query(onlyPublished);

        var post = await query.FirstOrDefaultAsync(post => post.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

        return post;
    }

    public Task<Post> GetById(Guid id)
    {
        return Query(false).FirstAsync(x => x.Id == id);
    }

    public async Task<Post> Create(Post post)
    {
        dbContext.Posts.Add(post);
        await dbContext.SaveChangesAsync();
        return post;
    }

    public async Task<Post> Update(Post post)
    {
        dbContext.Posts.Update(post);
        await dbContext.SaveChangesAsync();
        return post;
    }

    public async Task Delete(Post post)
    {
        dbContext.Posts.Remove(post);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<string>> GetSlugsStartingWith(string commonPart)
    {
        var pattern = $"{commonPart}%".ToLowerInvariant();
        var query = Query(false).Where(post => EF.Functions.Like(post.Slug, pattern));

        var slugs = await query.Select(post => post.Slug).ToListAsync();
        return slugs;
    }

    public Task<Post> GetBySlug(string slug, bool onlyPublished)
    {
        return Query(onlyPublished).FirstAsync(x => x.Slug == slug);
    }

    private IQueryable<Post> Query(bool onlyPublished)
    {
        if (onlyPublished)
        {
            return dbContext.Posts.Where(post => post.IsPublished);
        }

        return dbContext.Posts;
    }
}
