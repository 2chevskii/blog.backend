namespace Dvchevskii.Blog.Api.Admin.Models;

public record PostEditModel(
    Guid Id,
    string Slug,
    string Title,
    string? Tagline,
    string Body,
    bool IsPublished,
    PostEditorModel CreatedBy,
    PostEditorModel? UpdatedBy,
    Guid? HeaderImageId,
    Uri? HeaderImageUrl
);