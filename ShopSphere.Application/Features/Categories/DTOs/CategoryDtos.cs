using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Application.Features.Categories.DTOs;

public record CategoryResponseDto(
    int Id,
    string Name,
    string Slug,
    string? Description,
    string? ImageUrl,
    int DisplayOrder,
    bool IsActive,
    int? ParentCategoryId,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);

public record CreateCategoryRequest(
    [Required, StringLength(100, MinimumLength = 2)] string Name,
    [Required, RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase alphanumeric with hyphens")] string Slug,
    [StringLength(500)] string? Description,
    string? ImageUrl,
    int DisplayOrder = 0,
    bool IsActive = true,
    int? ParentCategoryId = null
);

public record UpdateCategoryRequest(
    [Required, StringLength(100, MinimumLength = 2)] string Name,
    [Required, RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase alphanumeric with hyphens")] string Slug,
    [StringLength(500)] string? Description,
    string? ImageUrl,
    int DisplayOrder = 0,
    bool IsActive = true,
    int? ParentCategoryId = null
);
