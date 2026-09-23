using ShopSphere.Domain;

namespace ShopSphere.Application.IService
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
    }
}