using Dvchevskii.Blog.Entities.Common;
using Dvchevskii.Blog.Shared.Contracts.Authentication.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Dvchevskii.Blog.Infrastructure;

internal class AuditInfoInterceptor(IAuthenticationContext authenticationContext) : SaveChangesInterceptor
{
    /*public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        return base.SavingChanges(eventData, result);
    }*/

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        if (eventData.Context == null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        if (!authenticationContext.IsAuthenticated)
        {
            throw new InvalidOperationException("Authenticated context expected");
        }

        var actorId = authenticationContext.UserId;

        foreach (var entityEntry in eventData.Context.ChangeTracker.Entries<Entity>().ToArray())
        {
            var entity = entityEntry.Entity;

            switch (entityEntry.State)
            {
                case EntityState.Added:
                {
                    entity.AuditInfo.CreatedAt = DateTime.UtcNow;
                    entity.AuditInfo.CreatedBy = actorId;

                    var auditLogEntry = new AuditLogEntry
                    {
                        Id = Guid.NewGuid(),
                        Type = AuditEventType.Created,
                        EntityId = entity.Id,
                        EntityType = entityEntry.Metadata.GetTableName() ?? entityEntry.Metadata.Name,
                        Timestamp = DateTime.UtcNow,
                        ActorId = actorId
                    };
                    eventData.Context.Add(auditLogEntry);
                    break;
                }
                case EntityState.Modified:
                {
                    entity.AuditInfo.UpdatedAt = DateTime.UtcNow;
                    entity.AuditInfo.UpdatedBy = actorId;

                    var auditLogEntry = new AuditLogEntry
                    {
                        Id = Guid.NewGuid(),
                        Type = AuditEventType.Updated,
                        EntityId = entity.Id,
                        EntityType = entityEntry.Metadata.GetTableName() ?? entityEntry.Metadata.Name,
                        Timestamp = DateTime.UtcNow,
                        ActorId = actorId
                    };
                    eventData.Context.Add(auditLogEntry);
                    break;
                }
                case EntityState.Deleted:
                {
                    entityEntry.State = EntityState.Modified;

                    entity.AuditInfo.IsDeleted = true;
                    entity.AuditInfo.DeletedAt = DateTime.UtcNow;
                    entity.AuditInfo.DeletedBy = actorId;

                    var entityAuditInfoEntry = eventData.Context.ChangeTracker.Entries<AuditInfo>()
                        .First(x => x.Entity == entity.AuditInfo);
                    entityAuditInfoEntry.State = EntityState.Modified;

                    var auditLogEntry = new AuditLogEntry
                    {
                        Id = Guid.NewGuid(),
                        Type = AuditEventType.Deleted,
                        EntityId = entity.Id,
                        EntityType = entityEntry.Metadata.GetTableName() ?? entityEntry.Metadata.Name,
                        Timestamp = DateTime.UtcNow,
                        ActorId = actorId
                    };
                    eventData.Context.Add(auditLogEntry);
                    break;
                }
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
