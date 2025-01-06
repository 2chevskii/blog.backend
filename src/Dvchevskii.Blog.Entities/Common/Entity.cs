namespace Dvchevskii.Blog.Entities.Common;

public abstract class Entity
{
    public required Guid Id { get; init; }
    public AuditInfo AuditInfo { get; set; } = new AuditInfo();
}
