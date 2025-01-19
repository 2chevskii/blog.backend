using System.Linq.Expressions;
using Dvchevskii.Blog.Application.Contracts.ValueObjects.Pagination;
using Dvchevskii.Blog.Core.Entities.Posts;

namespace Dvchevskii.Blog.Core.Repositories.Posts;

public interface IPostRepository
{
    Task<List<Post>> GetList(bool onlyPublished);

    Task<LimitedQueryResult<Post>> GetList(LimitedQuerySettings settings, bool onlyPublished);

    Task<LimitedQueryResult<T>> GetList<T>(LimitedQuerySettings settings, bool onlyPublished,
        Expression<Func<Post, T>> projectionExpression);

    Task<Post?> FindBySlug(string slug, bool onlyPublished = true);
    Task<Post> GetById(Guid id);

    Task<Post> Create(Post post);
    Task<Post> Update(Post post);
    Task Delete(Post post);

    Task<List<string>> GetSlugsStartingWith(string commonPart);
    Task<Post> GetBySlug(string slug, bool onlyPublished);
}
