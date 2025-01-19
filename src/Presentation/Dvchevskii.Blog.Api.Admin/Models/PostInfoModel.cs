namespace Dvchevskii.Blog.Api.Admin.Models;

public class PostInfoModel
{
    public Guid Id { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public bool IsPublished { get; set; }
    public PostEditorModel CreatedBy { get; set; }
    public PostEditorModel? UpdatedBy { get; set; }
}
