﻿using Dvchevskii.Blog.Entities.Authentication.Accounts;
using Dvchevskii.Blog.Entities.Authentication.Users;
using Dvchevskii.Blog.Entities.Common;
using Dvchevskii.Blog.Entities.Files;
using Dvchevskii.Blog.Entities.Posts;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Infrastructure;

public sealed class BlogDbContext(DbContextOptions<BlogDbContext> options)
    : DbContext(options), IDataProtectionKeyContext
{
    public DbSet<AuditLogEntry> AuditLogEntries => Set<AuditLogEntry>();
    public DbSet<User> Users => Set<User>();
    public DbSet<PasswordAccount> PasswordAccounts => Set<PasswordAccount>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
    public DbSet<Image> Images => Set<Image>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Owned<AuditInfo>();
        modelBuilder.Entity<AuditLogEntry>()
            .HasOne(x => x.Actor)
            .WithMany()
            .HasForeignKey(x => x.ActorId);
        modelBuilder.Entity<AuditLogEntry>()
            .Property(x => x.Type)
            .HasConversion<string>(
                e => e.ToString("G").ToLowerInvariant(),
                d => Enum.Parse<AuditEventType>(d, true)
            );
    }
}
