namespace Dvchevskii.Blog.Admin.Models;

public record CreatePostRequest(
    string Title,
    string? Tagline,
    string Body,
    bool IsPublished,
    Guid? HeaderImageId
);
