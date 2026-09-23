using ShopSphere.Application.Features.Categories;
using ShopSphere.Domain;

namespace ShopSphere.Infrastructure.Persistence.Repositories;

public class CategoryRepository : InMemoryRepository<Category>, ICategoryRepository
{
    public CategoryRepository()
    {
        SeedInitialData();
    }

    public Task<Category?> GetBySlugAsync(string slug)
    {
        var category = _entities.Values.FirstOrDefault(c =>
            string.Equals(c.Slug, slug, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(category);
    }

    public Task<IReadOnlyList<Category>> GetSubCategoriesAsync(int parentCategoryId)
    {
        IReadOnlyList<Category> list = _entities.Values
            .Where(c => c.ParentCategoryId == parentCategoryId)
            .OrderBy(c => c.DisplayOrder)
            .ToList();
        return Task.FromResult(list);
    }

    public Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
    {
        var exists = _entities.Values.Any(c =>
            string.Equals(c.Slug, slug, StringComparison.OrdinalIgnoreCase) &&
            (!excludeId.HasValue || c.Id != excludeId.Value));
        return Task.FromResult(exists);
    }

    private void SeedInitialData()
    {
        var seededCategories = new List<Category>
        {
            new()
            {
                Id = 1,
                Name = "Electronics",
                Slug = "electronics",
                Description = "Electronic gadgets, computers, and devices",
                DisplayOrder = 1,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = 2,
                Name = "Laptops & Computers",
                Slug = "laptops-computers",
                Description = "Workstation laptops, gaming PCs, and ultrabooks",
                ParentCategoryId = 1,
                DisplayOrder = 1,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = 3,
                Name = "Audio & Headphones",
                Slug = "audio-headphones",
                Description = "Wireless earbuds, over-ear studio monitors, and soundbars",
                ParentCategoryId = 1,
                DisplayOrder = 2,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = 4,
                Name = "Home & Kitchen",
                Slug = "home-kitchen",
                Description = "Smart home appliances and kitchen essentials",
                DisplayOrder = 2,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            }
        };

        foreach (var category in seededCategories)
        {
            AddAsync(category).GetAwaiter().GetResult();
        }
    }
}
