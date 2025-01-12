namespace Dvchevskii.Blog.Admin.Models;

public record UpdatePostRequest(
    Guid Id,
    string Title,
    string? Tagline,
    string Body,
    bool IsPublished,
    Guid? HeaderImageId
);
