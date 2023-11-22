using Application.Features.Articles;
using Application.Features.Categories;
using Application.Features.Revisions;
using Microsoft.EntityFrameworkCore;

namespace Application.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Revision> Revisions { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
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