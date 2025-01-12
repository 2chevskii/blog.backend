using Dvchevskii.Blog.Core.Entities.Authentication.Users;

namespace Dvchevskii.Blog.Core.Entities.Common;

public sealed class AuditLogEntry
{
    public required Guid Id { get; init; }
    public required Guid EntityId { get; init; }
    public required string EntityType { get; init; }
    public required AuditEventType Type { get; init; }
    public required DateTime Timestamp { get; init; }
    public required Guid ActorId { get; init; }

    public User Actor { get; set; }
}
