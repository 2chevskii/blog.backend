namespace Dvchevskii.Blog.Admin.Models;

public record PostEditModel(
    Guid Id,
    string Slug,
    string Title,
    string? Tagline,
    string Body,
    bool IsPublished,
    PostAuthorInfo CreatedBy,
    PostAuthorInfo? UpdatedBy,
    Guid? HeaderImageId,
    Uri? HeaderImageUrl
);