using Microsoft.EntityFrameworkCore;
using ShopSphere.Domain;

namespace ShopSphere.Infrastructure.Persistence;

public class ShopSphereDbContext : DbContext
{
    public ShopSphereDbContext(DbContextOptions<ShopSphereDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations automatically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopSphereDbContext).Assembly);
    }
}
