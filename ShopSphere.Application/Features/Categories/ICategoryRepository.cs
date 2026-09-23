using ShopSphere.Application.Common.Interfaces;
using ShopSphere.Domain;

namespace ShopSphere.Application.Features.Categories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<Category>> GetSubCategoriesAsync(int parentCategoryId);
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
}
