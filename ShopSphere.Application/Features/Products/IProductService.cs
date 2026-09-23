using ShopSphere.Application.Features.Products.DTOs;

namespace ShopSphere.Application.Features.Products;

public interface IProductService
{
    Task<IReadOnlyList<ProductResponseDto>> GetAllAsync(int? categoryId = null, bool? onlyActive = null, bool? onlyFeatured = null);
    Task<ProductResponseDto?> GetByIdAsync(int id);
    Task<ProductResponseDto?> GetBySlugAsync(string slug);
    Task<ProductResponseDto?> GetBySkuAsync(string sku);
    Task<ProductResponseDto> CreateAsync(CreateProductRequest request);
    Task<ProductResponseDto?> UpdateAsync(int id, UpdateProductRequest request);
    Task<bool> DeleteAsync(int id);
}
