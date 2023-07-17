using MyMovieProjectApi.Models;

namespace MyMovieProjectApi.Interfaces
{
    public interface IMovieRepository
    {
        Task<MovieAddResponse> AddMovie(int page);
        Task<MovieRatingResponse> AddRatingMovie(MovieRatingRequest request);
        MovieListResponse GetMovies(int pageSize);
        MovieGetResponse GetMovie(int id);
        MovieSendMailResponse SendMailMovie(MovieSendMailRequest request);

    }
}
