using ShopSphere.Application.Features.Categories.DTOs;

namespace ShopSphere.Application.Features.Categories;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync(bool onlyActive = false);
    Task<CategoryResponseDto?> GetByIdAsync(int id);
    Task<CategoryResponseDto?> GetBySlugAsync(string slug);
    Task<CategoryResponseDto> CreateAsync(CreateCategoryRequest request);
    Task<CategoryResponseDto?> UpdateAsync(int id, UpdateCategoryRequest request);
    Task<bool> DeleteAsync(int id);
}
