using ShopSphere.Application.Features.Products;
using ShopSphere.Domain;

namespace ShopSphere.Infrastructure.Persistence.Repositories;

public class ProductRepository : InMemoryRepository<Product>, IProductRepository
{
    public ProductRepository()
    {
        SeedInitialData();
    }

    public Task<Product?> GetBySlugAsync(string slug)
    {
        var product = _entities.Values.FirstOrDefault(p =>
            string.Equals(p.Slug, slug, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(product);
    }

    public Task<Product?> GetBySkuAsync(string sku)
    {
        var product = _entities.Values.FirstOrDefault(p =>
            string.Equals(p.Sku, sku, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(product);
    }

    public Task<IReadOnlyList<Product>> GetByCategoryIdAsync(int categoryId)
    {
        IReadOnlyList<Product> list = _entities.Values
            .Where(p => p.CategoryId == categoryId)
            .OrderByDescending(p => p.CreatedAtUtc)
            .ToList();
        return Task.FromResult(list);
    }

    public Task<IReadOnlyList<Product>> GetFeaturedAsync()
    {
        IReadOnlyList<Product> list = _entities.Values
            .Where(p => p.IsFeatured && p.IsActive)
            .OrderByDescending(p => p.CreatedAtUtc)
            .ToList();
        return Task.FromResult(list);
    }

    public Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
        var exists = _entities.Values.Any(p =>
            string.Equals(p.Slug, slug, StringComparison.OrdinalIgnoreCase) &&
            (!excludeId.HasValue || p.Id != excludeId.Value));
        return Task.FromResult(exists);
    }

    public Task<bool> SkuExistsAsync(string sku, int? excludeId = null)
    {
        var exists = _entities.Values.Any(p =>
            string.Equals(p.Sku, sku, StringComparison.OrdinalIgnoreCase) &&
            (!excludeId.HasValue || p.Id != excludeId.Value));
        return Task.FromResult(exists);
    }

    private void SeedInitialData()
    {
        var seededProducts = new List<Product>
        {
            new()
            {
                Id = 1,
                Sku = "PROD-MBP16-M3",
                Name = "MacBook Pro 16\" M3 Max",
                Slug = "macbook-pro-16-m3-max",
                ShortDescription = "Apple MacBook Pro 16-inch with M3 Max chip, 36GB Unified Memory, 1TB SSD.",
                Description = "High performance laptop engineered for pro developers and creative workflows.",
                Price = 3499.00m,
                CompareAtPrice = 3699.00m,
                CostPrice = 2800.00m,
                StockQuantity = 25,
                LowStockThreshold = 5,
                WeightKg = 2.16m,
                IsActive = true,
                IsFeatured = true,
                CategoryId = 2,
                CreatedAtUtc = DateTime.UtcNow.AddDays(-10)
            },
            new()
            {
                Id = 2,
                Sku = "PROD-SONY-WH1000XM5",
                Name = "Sony WH-1000XM5 Wireless Headphones",
                Slug = "sony-wh-1000xm5-wireless-headphones",
                ShortDescription = "Industry leading noise-canceling headphones with premium sound.",
                Description = "Two processors and 8 microphones for unprecedented noise cancellation and exceptional call quality.",
                Price = 398.00m,
                CompareAtPrice = 449.99m,
                CostPrice = 250.00m,
                StockQuantity = 50,
                LowStockThreshold = 10,
                WeightKg = 0.25m,
                IsActive = true,
                IsFeatured = true,
                CategoryId = 3,
                CreatedAtUtc = DateTime.UtcNow.AddDays(-8)
            },
            new()
            {
                Id = 3,
                Sku = "PROD-DELL-XPS15",
                Name = "Dell XPS 15 OLED",
                Slug = "dell-xps-15-oled",
                ShortDescription = "15.6-inch 3.5K OLED touchscreen with Intel Core i9 and RTX 4070.",
                Description = "Stunning display with high computing horsepower in an ultra-slim aluminum chassis.",
                Price = 2399.00m,
                CompareAtPrice = null,
                CostPrice = 1900.00m,
                StockQuantity = 12,
                LowStockThreshold = 3,
                WeightKg = 1.92m,
                IsActive = true,
                IsFeatured = false,
                CategoryId = 2,
                CreatedAtUtc = DateTime.UtcNow.AddDays(-5)
            }
        };

        foreach (var product in seededProducts)
        {
            AddAsync(product).GetAwaiter().GetResult();
        }
    }
}
