using Azure;
using Microsoft.EntityFrameworkCore;
using MyMovieProjectApi.Data;
using MyMovieProjectApi.Interfaces;
using MyMovieProjectApi.Models;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyMovieProjectApi.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext context;
        public MovieRepository(MovieContext context)
        {
            this.context = context;
        }
       public MovieGetResponse GetMovie(int id)
        {
            var response = new MovieGetResponse();
            var movie = context.Movie.FirstOrDefault(p => p.id == id);

            if (movie == null)
            {
                response.errorMessage = "The movie is not found";
                response.isSuccess = false;
                return response;

            }
            var movieRating = context.Rating.Where(x => x.movieId == id).ToList();

            if (movieRating.Count > 1)
            {
                var ratingValue = movieRating.Select(x => x.ratingValue).ToList();
                double avg = (double)((ratingValue == null) ? 0 : ratingValue.Average());
                response.RatingAverage = avg;
            }

            response.result = movie;
            response.movieRating = movieRating;
            response.isSuccess = true;
            return response;


        }

        public MovieListResponse GetMovies(int page)
        {
            var response = new MovieListResponse();
            var skip = (page - 1) * 20;
            response.results = context.Movie.Skip(skip).Take(20).ToList();
            response.page = page;
            response.isSuccess = true;
            return response;
        }

       public async Task<MovieAddResponse> AddMovie(int pageSize)
        {
            var movieResponse = new MovieAddResponse();
            var options = new RestClientOptions("https://api.themoviedb.org/3/movie/popular");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddParameter("page", pageSize);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwM2EzYTZjZDJjMzg0OWEyZDQ3MzdhNGJkMjlmZTM5ZSIsInN1YiI6IjY0YjA2MWI3YzQ5MDQ4MDBhYzk2ZDQ3NSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.0Q3LdXXzzjDhUei_ofUxgF2q3vk2lpP-JYzpQ8D-WFQ");
            var response = await client.GetAsync(request);
            if (response.IsSuccessful)
            {
                var content = JsonSerializer.Deserialize<MovieAddResponse>(response.Content);
                foreach (var item in content.results)
                {
                    var movie = context.Movie.FirstOrDefault(x => x.id == item.id);
                    if (movie != null)
                    {
                        movieResponse.isSuccess = false;
                        movieResponse.errorMessage = "Already exist";
                        return movieResponse;
                    }
                    context.Movie.Add(item);
                    context.SaveChanges();
                }
              

            }
            movieResponse.isSuccess = true;
            movieResponse.page = pageSize;
            return movieResponse;

        }

        public MovieSendMailResponse SendMailMovie(MovieSendMailRequest request)
        {
            
            var response = new MovieSendMailResponse();
            try
            {

                var movie = context.Movie.FirstOrDefault(p => p.id == request.movieId);

                if (movie == null)
                {
                    response.errorMessage = "The movie is not found";
                    response.isSuccess = false;
                    return response;

                }
                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "smtp.gmail.com";
                sc.UseDefaultCredentials = false;
                sc.EnableSsl = true;

                //Gönderecek mail hesabının bilgileri
                sc.Credentials = new NetworkCredential("bsrpekoz@gmail.com", "generated_password"); // "generated_password" kısmına google uygulama şifrelerinde oluşturulan 16 karakterlik şifre yazılır.

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("bsrpekoz@gmail.com", "Busrapekoz");

                mail.To.Add(request.email);


                mail.Subject = "Recomention Movie For You";
                mail.IsBodyHtml = true;
                mail.Body = $"<h1>You should watch this movie released on {movie.release_date.Date.ToString("yyyy-MM-dd")} ! </h1> <br> <strong> Movie Name: </strong> {movie.title} <br> <strong>Movie Overview: </strong> {movie.overview} <br> <h3>Enjoy watching! </h3>";
                sc.Send(mail);

                response.message = "Mail is successful";
                response.isSuccess = true;
            }
            catch(Exception ex) { 
            response.errorMessage = ex.Message;
            response.isSuccess = false;
            }

            return response;

        }

        public async Task<MovieRatingResponse> AddRatingMovie(MovieRatingRequest request)
        {
            var movieRatingResponse = new MovieRatingResponse();

            var movie = context.Movie.FirstOrDefault(p => p.id == request.movieId);

            if (movie == null)
            {
                movieRatingResponse.errorMessage = "The movie is not found.";
                movieRatingResponse.isSuccess = false;
                return movieRatingResponse;

            }
   
            var addRating = new Rating
            {
                movieId = request.movieId,
                ratingValue = request.ratingValue,
                note = request.note
            };

            if (addRating.ratingValue < 1 || addRating.ratingValue > 10)
            {
                movieRatingResponse.errorMessage = "Rating must be between 1 and 10.";
                return movieRatingResponse;
            }
            context.Rating.Add(addRating);
            await context.SaveChangesAsync();

            movieRatingResponse.isSuccess = true;
            return movieRatingResponse;

        }
         
    }
}
