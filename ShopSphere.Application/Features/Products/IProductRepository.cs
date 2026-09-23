using ShopSphere.Application.Common.Interfaces;
using ShopSphere.Domain;

namespace ShopSphere.Application.Features.Products;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetBySlugAsync(string slug);
    Task<Product?> GetBySkuAsync(string sku);
    Task<IReadOnlyList<Product>> GetByCategoryIdAsync(int categoryId);
    Task<IReadOnlyList<Product>> GetFeaturedAsync();
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
    Task<bool> SkuExistsAsync(string sku, int? excludeId = null);
}
