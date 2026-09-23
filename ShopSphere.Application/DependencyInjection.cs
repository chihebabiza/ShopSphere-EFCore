using Microsoft.Extensions.DependencyInjection;
using ShopSphere.Application.Features.Categories;
using ShopSphere.Application.Features.Products;

namespace ShopSphere.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Category feature
        services.AddScoped<ICategoryService, CategoryService>();

        // Product feature
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
