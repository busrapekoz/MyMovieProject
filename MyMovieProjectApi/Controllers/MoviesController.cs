using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMovieProjectApi.Interfaces;
using MyMovieProjectApi.Models;

namespace MyMovieProjectApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository movieRepository;
        public MoviesController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        [HttpPost]
        [Route("pageSize")]
        public async Task<MovieAddResponse> AddMovies(int pageSize) 
        {
            
            var response = await movieRepository.AddMovie(pageSize);
            return response;
        }

        [HttpGet]
        [Route("page")]
        public MovieListResponse GetMovies(int page)
        {

            var response =  movieRepository.GetMovies(page);
            return response;
        }

        [HttpGet]
        [Route("{id}")]
        public MovieGetResponse GetMovie(int id)
        {

            var response = movieRepository.GetMovie(id);
            return response;
        }

        [HttpPost]
        [Route("sendMail")]

        public MovieSendMailResponse SendMailMovie(MovieSendMailRequest request)
        {

            var response = movieRepository.SendMailMovie(request);
            return response;
        }

        [HttpPost]
        [Route("movieRating")]

        public async Task<MovieRatingResponse> AddRatingMovie(MovieRatingRequest request)
        {

            var response = await movieRepository.AddRatingMovie(request);
            return response;
        }
    }
}
