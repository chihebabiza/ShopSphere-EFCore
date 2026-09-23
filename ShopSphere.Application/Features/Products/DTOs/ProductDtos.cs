using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Application.Features.Products.DTOs;

public record ProductResponseDto(
    int Id,
    string Sku,
    string Name,
    string Slug,
    string? ShortDescription,
    string? Description,
    decimal Price,
    decimal? CompareAtPrice,
    decimal? CostPrice,
    int StockQuantity,
    int LowStockThreshold,
    decimal? WeightKg,
    bool IsActive,
    bool IsFeatured,
    int CategoryId,
    string? CategoryName,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);

public record CreateProductRequest(
    [Required, StringLength(50, MinimumLength = 3)] string Sku,
    [Required, StringLength(200, MinimumLength = 2)] string Name,
    [Required, RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase alphanumeric with hyphens")] string Slug,
    [StringLength(500)] string? ShortDescription,
    string? Description,
    [Range(0.01, 1000000.00)] decimal Price,
    [Range(0.01, 1000000.00)] decimal? CompareAtPrice,
    [Range(0.00, 1000000.00)] decimal? CostPrice,
    [Range(0, 1000000)] int StockQuantity = 0,
    [Range(0, 1000)] int LowStockThreshold = 5,
    [Range(0.001, 1000.0)] decimal? WeightKg = null,
    bool IsActive = true,
    bool IsFeatured = false,
    [Required] int CategoryId = 0
);

public record UpdateProductRequest(
    [Required, StringLength(50, MinimumLength = 3)] string Sku,
    [Required, StringLength(200, MinimumLength = 2)] string Name,
    [Required, RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase alphanumeric with hyphens")] string Slug,
    [StringLength(500)] string? ShortDescription,
    string? Description,
    [Range(0.01, 1000000.00)] decimal Price,
    [Range(0.01, 1000000.00)] decimal? CompareAtPrice,
    [Range(0.00, 1000000.00)] decimal? CostPrice,
    [Range(0, 1000000)] int StockQuantity = 0,
    [Range(0, 1000)] int LowStockThreshold = 5,
    [Range(0.001, 1000.0)] decimal? WeightKg = null,
    bool IsActive = true,
    bool IsFeatured = false,
    [Required] int CategoryId = 0
);
