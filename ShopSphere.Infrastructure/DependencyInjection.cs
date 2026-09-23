using Microsoft.Extensions.DependencyInjection;
using ShopSphere.Application.Features.Categories;
using ShopSphere.Application.Features.Products;
using ShopSphere.Application.IRepository;
using ShopSphere.Infrastructure.Persistence.Repositories;
using ShopSphere.Infrastructure.Repository;

namespace ShopSphere.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // In-memory repositories registered as Singleton to preserve state in-memory across HTTP requests
        services.AddSingleton<ICategoryRepository, CategoryRepository>();
        services.AddSingleton<IProductRepository, ProductRepository>();

        // Legacy / existing repositories
        services.AddScoped<IMovieRepository, MovieRepository>();

        return services;
    }
}
