using ShopSphere.Application.IRepository;
using ShopSphere.Application.IService;
using ShopSphere.Domain;

namespace ShopSphere.Application.Service
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository movieRepository;

        public MovieService(IMovieRepository _movieRepository)
        {
            movieRepository = _movieRepository;
        }
        public List<Movie> GetAllMovies()
        {
            var movies = movieRepository.GetAllMovies();

            return movies;
        }
    }
}