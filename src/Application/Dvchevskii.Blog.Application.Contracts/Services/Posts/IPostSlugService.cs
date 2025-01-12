namespace Dvchevskii.Blog.Application.Contracts.Services.Posts;

public interface IPostSlugService
{
    Task<string> GetAvailableSlug(string title);
}
