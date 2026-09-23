using ShopSphere.Application.Features.Categories.DTOs;
using ShopSphere.Domain;

namespace ShopSphere.Application.Features.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync(bool onlyActive = false)
    {
        var categories = await _categoryRepository.GetAllAsync();
        var query = categories.AsEnumerable();

        if (onlyActive)
        {
            query = query.Where(c => c.IsActive);
        }

        return query
            .OrderBy(c => c.DisplayOrder)
            .ThenBy(c => c.Name)
            .Select(MapToDto)
            .ToList();
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category == null ? null : MapToDto(category);
    }

    public async Task<CategoryResponseDto?> GetBySlugAsync(string slug)
    {
        var category = await _categoryRepository.GetBySlugAsync(slug);
        return category == null ? null : MapToDto(category);
    }

    public async Task<CategoryResponseDto> CreateAsync(CreateCategoryRequest request)
    {
        var normalizedSlug = request.Slug.Trim().ToLowerInvariant();

        if (await _categoryRepository.SlugExistsAsync(normalizedSlug))
        {
            throw new InvalidOperationException($"Category with slug '{normalizedSlug}' already exists.");
        }

        if (request.ParentCategoryId.HasValue && !await _categoryRepository.ExistsAsync(request.ParentCategoryId.Value))
        {
            throw new ArgumentException($"Parent category with ID '{request.ParentCategoryId.Value}' does not exist.");
        }

        var category = new Category
        {
            Name = request.Name.Trim(),
            Slug = normalizedSlug,
            Description = request.Description?.Trim(),
            ImageUrl = request.ImageUrl?.Trim(),
            DisplayOrder = request.DisplayOrder,
            IsActive = request.IsActive,
            ParentCategoryId = request.ParentCategoryId,
            CreatedAtUtc = DateTime.UtcNow
        };

        var created = await _categoryRepository.AddAsync(category);
        return MapToDto(created);
    }

    public async Task<CategoryResponseDto?> UpdateAsync(int id, UpdateCategoryRequest request)
    {
        var existing = await _categoryRepository.GetByIdAsync(id);
        if (existing == null)
        {
            return null;
        }

        var normalizedSlug = request.Slug.Trim().ToLowerInvariant();
        if (await _categoryRepository.SlugExistsAsync(normalizedSlug, excludeId: id))
        {
            throw new InvalidOperationException($"Category with slug '{normalizedSlug}' already exists.");
        }

        if (request.ParentCategoryId.HasValue)
        {
            if (request.ParentCategoryId.Value == id)
            {
                throw new InvalidOperationException("A category cannot be its own parent.");
            }

            if (!await _categoryRepository.ExistsAsync(request.ParentCategoryId.Value))
            {
                throw new ArgumentException($"Parent category with ID '{request.ParentCategoryId.Value}' does not exist.");
            }
        }

        existing.Name = request.Name.Trim();
        existing.Slug = normalizedSlug;
        existing.Description = request.Description?.Trim();
        existing.ImageUrl = request.ImageUrl?.Trim();
        existing.DisplayOrder = request.DisplayOrder;
        existing.IsActive = request.IsActive;
        existing.ParentCategoryId = request.ParentCategoryId;
        existing.UpdatedAtUtc = DateTime.UtcNow;

        await _categoryRepository.UpdateAsync(existing);
        return MapToDto(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _categoryRepository.DeleteAsync(id);
    }

    private static CategoryResponseDto MapToDto(Category category)
    {
        return new CategoryResponseDto(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.ImageUrl,
            category.DisplayOrder,
            category.IsActive,
            category.ParentCategoryId,
            category.CreatedAtUtc,
            category.UpdatedAtUtc
        );
    }
}
