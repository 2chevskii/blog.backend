namespace Dvchevskii.Blog.Admin.Models;

public class PostInfoModel
{
    public Guid Id { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public bool IsPublished { get; set; }
    public PostAuthorInfo CreatedBy { get; set; }
    public PostAuthorInfo? UpdatedBy { get; set; }
    public Uri? HeaderImageUrl { get; set; }
}
