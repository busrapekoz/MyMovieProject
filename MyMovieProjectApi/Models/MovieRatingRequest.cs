namespace MyMovieProjectApi.Models
{
    public class MovieRatingRequest
    {
        public int movieId { get; set; }
        public int? ratingValue { get; set; }
        public string note { get; set; }
    }
}
