using ShopSphere.Application.Features.Categories;
using ShopSphere.Application.Features.Products.DTOs;
using ShopSphere.Domain;

namespace ShopSphere.Application.Features.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyList<ProductResponseDto>> GetAllAsync(int? categoryId = null, bool? onlyActive = null, bool? onlyFeatured = null)
    {
        var products = await _productRepository.GetAllAsync();
        var query = products.AsEnumerable();

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (onlyActive.HasValue)
        {
            query = query.Where(p => p.IsActive == onlyActive.Value);
        }

        if (onlyFeatured.HasValue)
        {
            query = query.Where(p => p.IsFeatured == onlyFeatured.Value);
        }

        var categories = (await _categoryRepository.GetAllAsync()).ToDictionary(c => c.Id, c => c.Name);

        return query
            .OrderByDescending(p => p.CreatedAtUtc)
            .Select(p => MapToDto(p, categories.GetValueOrDefault(p.CategoryId)))
            .ToList();
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;

        var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
        return MapToDto(product, category?.Name);
    }

    public async Task<ProductResponseDto?> GetBySlugAsync(string slug)
    {
        var product = await _productRepository.GetBySlugAsync(slug);
        if (product == null) return null;

        var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
        return MapToDto(product, category?.Name);
    }

    public async Task<ProductResponseDto?> GetBySkuAsync(string sku)
    {
        var product = await _productRepository.GetBySkuAsync(sku);
        if (product == null) return null;

        var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
        return MapToDto(product, category?.Name);
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductRequest request)
    {
        var normalizedSku = request.Sku.Trim().ToUpperInvariant();
        var normalizedSlug = request.Slug.Trim().ToLowerInvariant();

        if (!await _categoryRepository.ExistsAsync(request.CategoryId))
        {
            throw new ArgumentException($"Category with ID '{request.CategoryId}' does not exist.");
        }

        if (await _productRepository.SkuExistsAsync(normalizedSku))
        {
            throw new InvalidOperationException($"Product with SKU '{normalizedSku}' already exists.");
        }

        if (await _productRepository.SlugExistsAsync(normalizedSlug))
        {
            throw new InvalidOperationException($"Product with slug '{normalizedSlug}' already exists.");
        }

        var product = new Product
        {
            Sku = normalizedSku,
            Name = request.Name.Trim(),
            Slug = normalizedSlug,
            ShortDescription = request.ShortDescription?.Trim(),
            Description = request.Description?.Trim(),
            Price = request.Price,
            CompareAtPrice = request.CompareAtPrice,
            CostPrice = request.CostPrice,
            StockQuantity = request.StockQuantity,
            LowStockThreshold = request.LowStockThreshold,
            WeightKg = request.WeightKg,
            IsActive = request.IsActive,
            IsFeatured = request.IsFeatured,
            CategoryId = request.CategoryId,
            CreatedAtUtc = DateTime.UtcNow
        };

        var created = await _productRepository.AddAsync(product);
        var category = await _categoryRepository.GetByIdAsync(created.CategoryId);
        return MapToDto(created, category?.Name);
    }

    public async Task<ProductResponseDto?> UpdateAsync(int id, UpdateProductRequest request)
    {
        var existing = await _productRepository.GetByIdAsync(id);
        if (existing == null) return null;

        var normalizedSku = request.Sku.Trim().ToUpperInvariant();
        var normalizedSlug = request.Slug.Trim().ToLowerInvariant();

        if (!await _categoryRepository.ExistsAsync(request.CategoryId))
        {
            throw new ArgumentException($"Category with ID '{request.CategoryId}' does not exist.");
        }

        if (await _productRepository.SkuExistsAsync(normalizedSku, excludeId: id))
        {
            throw new InvalidOperationException($"Product with SKU '{normalizedSku}' already exists.");
        }

        if (await _productRepository.SlugExistsAsync(normalizedSlug, excludeId: id))
        {
            throw new InvalidOperationException($"Product with slug '{normalizedSlug}' already exists.");
        }

        existing.Sku = normalizedSku;
        existing.Name = request.Name.Trim();
        existing.Slug = normalizedSlug;
        existing.ShortDescription = request.ShortDescription?.Trim();
        existing.Description = request.Description?.Trim();
        existing.Price = request.Price;
        existing.CompareAtPrice = request.CompareAtPrice;
        existing.CostPrice = request.CostPrice;
        existing.StockQuantity = request.StockQuantity;
        existing.LowStockThreshold = request.LowStockThreshold;
        existing.WeightKg = request.WeightKg;
        existing.IsActive = request.IsActive;
        existing.IsFeatured = request.IsFeatured;
        existing.CategoryId = request.CategoryId;
        existing.UpdatedAtUtc = DateTime.UtcNow;

        await _productRepository.UpdateAsync(existing);
        var category = await _categoryRepository.GetByIdAsync(existing.CategoryId);
        return MapToDto(existing, category?.Name);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }

    private static ProductResponseDto MapToDto(Product product, string? categoryName = null)
    {
        return new ProductResponseDto(
            product.Id,
            product.Sku,
            product.Name,
            product.Slug,
            product.ShortDescription,
            product.Description,
            product.Price,
            product.CompareAtPrice,
            product.CostPrice,
            product.StockQuantity,
            product.LowStockThreshold,
            product.WeightKg,
            product.IsActive,
            product.IsFeatured,
            product.CategoryId,
            categoryName,
            product.CreatedAtUtc,
            product.UpdatedAtUtc
        );
    }
}
