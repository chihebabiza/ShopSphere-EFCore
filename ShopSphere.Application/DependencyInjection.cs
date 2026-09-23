using Microsoft.Extensions.DependencyInjection;
using ShopSphere.Application.Features.Categories;
using ShopSphere.Application.Features.Products;
using ShopSphere.Application.Features.Carts;

namespace ShopSphere.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();

        return services;
    }
}
