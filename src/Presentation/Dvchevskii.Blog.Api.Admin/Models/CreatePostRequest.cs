namespace Dvchevskii.Blog.Api.Admin.Models;

public record CreatePostRequest(
    string Title,
    string? Tagline,
    string Body,
    bool IsPublished,
    Guid? HeaderImageId
);
