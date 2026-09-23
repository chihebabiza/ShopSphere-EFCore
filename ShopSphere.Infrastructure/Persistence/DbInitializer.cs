using Microsoft.EntityFrameworkCore;
using ShopSphere.Domain;

namespace ShopSphere.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(ShopSphereDbContext context)
    {
        // Apply pending migrations automatically on startup
        await context.Database.MigrateAsync();

        if (!await context.Categories.AnyAsync())
        {
            var electronics = new Category
            {
                Name = "Electronics",
                Slug = "electronics",
                Description = "Electronic gadgets, computers, and devices",
                DisplayOrder = 1,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            var homeKitchen = new Category
            {
                Name = "Home & Kitchen",
                Slug = "home-kitchen",
                Description = "Smart home appliances and kitchen essentials",
                DisplayOrder = 2,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            await context.Categories.AddRangeAsync(electronics, homeKitchen);
            await context.SaveChangesAsync();

            var laptops = new Category
            {
                Name = "Laptops & Computers",
                Slug = "laptops-computers",
                Description = "Workstation laptops, gaming PCs, and ultrabooks",
                ParentCategoryId = electronics.Id,
                DisplayOrder = 1,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            var audio = new Category
            {
                Name = "Audio & Headphones",
                Slug = "audio-headphones",
                Description = "Wireless earbuds, over-ear studio monitors, and soundbars",
                ParentCategoryId = electronics.Id,
                DisplayOrder = 2,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            await context.Categories.AddRangeAsync(laptops, audio);
            await context.SaveChangesAsync();

            var products = new List<Product>
            {
                new()
                {
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
                    CategoryId = laptops.Id,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-10)
                },
                new()
                {
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
                    CategoryId = audio.Id,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-8)
                },
                new()
                {
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
                    CategoryId = laptops.Id,
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-5)
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
