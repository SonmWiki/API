using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Data;

public interface IApplicationDbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Revision> Revisions { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Author> Authors { get; set; }

    Task<int> SaveChangesAsync(CancellationToken token = default);
}