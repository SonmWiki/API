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
    public required DbSet<User> Users { get; set; }
    public required DbSet<Navigation> Navigations { get; set; }
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<Permission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_trgm");
        modelBuilder.Entity<Article>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(ApplicationConstants.MaxSlugLength);
            entity.Property(e => e.Title).HasMaxLength(ApplicationConstants.MaxTitleLength);
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
            entity.Property(e => e.Name).HasMaxLength(ApplicationConstants.MaxTitleLength);
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
            entity.Property(e => e.Name).HasMaxLength(ApplicationConstants.MaxTitleLength);
            entity.Property(e => e.Icon).HasMaxLength(ApplicationConstants.MaxTitleLength);
            entity.Property(e => e.Uri).HasMaxLength(ApplicationConstants.MaxUriLength);
            entity.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e=>e.ParentId)
                .OnDelete(DeleteBehavior.SetNull);
        });
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasMany(e => e.Users)
                .WithMany(e => e.Roles);

            entity.HasData(Domain.Rbac.Roles.All.Select(role => new Role
            {
                Id = role.Id,
                Name = role.Name
            }));

            entity.HasMany(e => e.Permissions)
                .WithMany(e => e.Roles)
                .UsingEntity(j => j.ToTable("RolePermission")
                    .HasData(
                        Domain.Rbac.Roles.All.SelectMany(role =>
                                Domain.Rbac.Roles.GetDefaultPermissions(role),
                            (role, permission) => new { RolesId = role.Id, PermissionsId = permission.Id }
                        )
                    ));
        });
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasData(Domain.Rbac.Permissions.All);
        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.ExternalId);
            entity.HasIndex(e => e.Name);
        });
    }
}