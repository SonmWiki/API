using Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ArticleCategory> ArticleCategories { get; set; }
    public DbSet<Revision> Revisions { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(128);
            entity.HasMany(e => e.Categories)
                .WithMany(e => e.Articles)
                .UsingEntity<ArticleCategory>();
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(128);
            entity.HasOne(e => e.Parent)
                .WithMany(e => e.SubCategories)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Revision>(entity =>
        {
            entity.Property(e => e.ReviewTimestamp).HasDefaultValue(null);
        });
    }
}