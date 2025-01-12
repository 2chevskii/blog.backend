using Dvchevskii.Blog.Core.Entities.Posts;
using Dvchevskii.Blog.Core.Repositories.Posts;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Infrastructure.Repositories.Posts;

internal class PostRepository(BlogDbContext dbContext) : IPostRepository
{
    public async Task<List<Post>> GetList(bool onlyPublished = true)
    {
        var posts = await Query(onlyPublished).ToListAsync();
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

    private IQueryable<Post> Query(bool onlyPublished)
    {
        if (onlyPublished)
        {
            return dbContext.Posts.Where(post => post.IsPublished);
        }

        return dbContext.Posts;
    }
}
