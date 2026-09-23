using Microsoft.EntityFrameworkCore;
using ShopSphere.Application.Features.Products;
using ShopSphere.Domain;

namespace ShopSphere.Infrastructure.Persistence.Repositories;

public class ProductRepository : EfRepository<Product>, IProductRepository
{

    public ProductRepository(AppDbContext context)
        : base(context)
    {
    }
    public async Task<Product?> GetBySlugAsync(string slug)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Sku == sku);
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryIdAsync(int categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .OrderByDescending(p => p.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Product>> GetFeaturedAsync()
    {
        return await _context.Products
            .Where(p => p.IsFeatured && p.IsActive)
            .OrderByDescending(p => p.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
        var exists = await _context.Products
            .AnyAsync(p =>
                p.Slug == slug &&
                (!excludeId.HasValue || p.Id != excludeId.Value));
        return exists;
    }

    public async Task<bool> SkuExistsAsync(string sku, int? excludeId = null)
    {
        var exists = await _context.Products
            .AnyAsync(p =>
                p.Sku == sku &&
                (!excludeId.HasValue || p.Id != excludeId.Value));
        return exists;
    }
}
