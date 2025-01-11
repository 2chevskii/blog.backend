using Dvchevskii.Blog.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Infrastructure;

internal class FilterByNonDeletedAttribute<T> : FilterByNonDeletedAttribute where T : Entity
{
    public override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<T>()
            .HasQueryFilter(e => !e.AuditInfo.IsDeleted);
    }
}
