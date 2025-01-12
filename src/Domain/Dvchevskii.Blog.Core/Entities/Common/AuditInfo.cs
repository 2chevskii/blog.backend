using System.ComponentModel.DataAnnotations.Schema;

namespace Dvchevskii.Blog.Core.Entities.Common;

public sealed class AuditInfo
{
    [Column("created_at")] public DateTime CreatedAt { get; set; }
    [Column("created_by")] public Guid CreatedBy { get; set; }
    [Column("updated_at")] public DateTime? UpdatedAt { get; set; }
    [Column("updated_by")] public Guid? UpdatedBy { get; set; }
    [Column("is_deleted")] public bool IsDeleted { get; set; }
    [Column("deleted_at")] public DateTime? DeletedAt { get; set; }
    [Column("deleted_by")] public Guid? DeletedBy { get; set; }
}
