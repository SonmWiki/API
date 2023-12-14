using Application.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ArticleCategory> ArticleCategories { get; set; }
    public DbSet<Revision> Revisions { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(512);
            entity.Property(e => e.Title).HasMaxLength(128);
            entity.HasMany(e => e.Categories)
                .WithMany(e => e.Articles)
                .UsingEntity<ArticleCategory>();
            entity.HasOne(e=>e.RedirectArticle)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(512);
            entity.Property(e => e.Name).HasMaxLength(128);
            entity.HasOne(e => e.Parent)
                .WithMany(e => e.SubCategories)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasMany(e => e.Articles)
                .WithMany(e => e.Categories)
                .UsingEntity<ArticleCategory>();
        });
        modelBuilder.Entity<Revision>(entity => { entity.Property(e => e.ReviewTimestamp).HasDefaultValue(null); });
    }
}