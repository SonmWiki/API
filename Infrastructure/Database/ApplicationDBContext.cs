using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class ApplicationDBContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Revision> Revisions { get; set; }

    public ApplicationDBContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasIndex(e => e.Path).IsUnique();
            entity.Property(e => e.Path).HasMaxLength(128);
            entity.Property(e => e.Weight).HasDefaultValue(0);
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Title).HasMaxLength(64);
            entity.Property(e => e.Weight).HasDefaultValue(0);
        });
        modelBuilder.Entity<Revision>(entity =>
        {
            entity.Property(e => e.Title).HasMaxLength(128);
            entity.Property(e => e.ReviewTimestamp).HasDefaultValue(null);
        });
    }
}