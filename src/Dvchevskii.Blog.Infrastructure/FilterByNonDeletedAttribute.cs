using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Infrastructure;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal abstract class FilterByNonDeletedAttribute : Attribute
{
    public abstract void OnModelCreating(ModelBuilder modelBuilder);
}
