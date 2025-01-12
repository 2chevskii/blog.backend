using Dvchevskii.Blog.Core.Entities.Posts;

namespace Dvchevskii.Blog.Core.Repositories.Posts;

public interface IPostRepository
{
    Task<List<Post>> GetList(bool onlyPublished = true);

    Task<Post?> FindBySlug(string slug, bool onlyPublished = true);
    Task<Post> GetById(Guid id);

    Task<Post> Create(Post post);
    Task<Post> Update(Post post);
    Task Delete(Post post);

    Task<List<string>> GetSlugsStartingWith(string commonPart);
}
