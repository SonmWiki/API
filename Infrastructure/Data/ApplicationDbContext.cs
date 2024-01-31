using Application.Common.Constants;
using Application.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IApplicationDbContext
{
    public required DbSet<Article> Articles { get; set; }
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<Revision> Revisions { get; set; }
    public required DbSet<Review> Reviews { get; set; }
    public required DbSet<Author> Authors { get; set; }
    public required DbSet<Navigation> Navigations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_trgm");
        modelBuilder.Entity<Article>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(ApplicationConstants.MaxSlugLength);
            entity.Property(e => e.Title).HasMaxLength(ApplicationConstants.MaxTitleLenght);
            entity.HasOne(e => e.RedirectArticle)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Revisions)
                .WithOne(e => e.Article)
                .HasForeignKey(e => e.ArticleId);
            entity.HasOne(e => e.CurrentRevision)
                .WithOne()
                .HasForeignKey<Article>(e => e.CurrentRevisionId);
            entity.HasIndex(e => e.Title)
                .HasMethod("GIST")
                .HasOperators("gist_trgm_ops");
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(ApplicationConstants.MaxSlugLength);
            entity.Property(e => e.Name).HasMaxLength(ApplicationConstants.MaxTitleLenght);
            entity.HasOne(e => e.Parent)
                .WithMany(e => e.SubCategories)
                .OnDelete(DeleteBehavior.SetNull);
        });
        modelBuilder.Entity<Revision>(entity =>
        {
            entity.HasMany(e => e.Reviews)
                .WithOne(e => e.Revision)
                .HasForeignKey(e => e.RevisionId);
            entity.HasOne(e => e.LatestReview)
                .WithOne()
                .HasForeignKey<Revision>(e => e.LatestReviewId);
        });
        modelBuilder.Entity<Navigation>(entity =>
        {
            entity.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e=>e.ParentId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}