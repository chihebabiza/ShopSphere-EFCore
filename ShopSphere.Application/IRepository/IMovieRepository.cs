using ShopSphere.Domain;

namespace ShopSphere.Application.IRepository
{
    public interface IMovieRepository
    {
        List<Movie> GetAllMovies();

    }
}