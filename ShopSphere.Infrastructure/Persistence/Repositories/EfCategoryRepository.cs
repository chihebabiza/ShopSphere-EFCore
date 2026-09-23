using Microsoft.EntityFrameworkCore;
using ShopSphere.Application.Features.Categories;
using ShopSphere.Domain;

namespace ShopSphere.Infrastructure.Persistence.Repositories;

public class EfCategoryRepository : EfRepository<Category>, ICategoryRepository
{
    public EfCategoryRepository(ShopSphereDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Slug == slug);
    }

    public async Task<IReadOnlyList<Category>> GetSubCategoriesAsync(int parentCategoryId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == parentCategoryId)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
        return await _dbSet
            .AnyAsync(c => c.Slug == slug && (!excludeId.HasValue || c.Id != excludeId.Value));
    }
}
