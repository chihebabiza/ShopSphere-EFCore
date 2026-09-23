using Microsoft.EntityFrameworkCore;
using ShopSphere.Application.Features.Categories;
using ShopSphere.Domain;

namespace ShopSphere.Infrastructure.Persistence.Repositories;

public class CategoryRepository : EfRepository<Category>, ICategoryRepository
{

    public CategoryRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Slug == slug);
    }

    public async Task<IReadOnlyList<Category>> GetSubCategoriesAsync(
        int parentCategoryId)
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryId == parentCategoryId)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();
    }

    public async Task<bool> SlugExistsAsync(
        string slug,
        int? excludeId = null)
    {
        return await _context.Categories
            .AnyAsync(c =>
                c.Slug == slug &&
                (!excludeId.HasValue || c.Id != excludeId.Value));
    }

}
