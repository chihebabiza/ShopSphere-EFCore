using Microsoft.Extensions.DependencyInjection;
using ShopSphere.Application.Features.Categories;
using ShopSphere.Application.Features.Products;
using ShopSphere.Application.IService;
using ShopSphere.Application.Service;

namespace ShopSphere.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Category feature
        services.AddScoped<ICategoryService, CategoryService>();

        // Product feature
        services.AddScoped<IProductService, ProductService>();

        // Legacy / existing services
        services.AddScoped<IMovieService, MovieService>();

        return services;
    }
}
