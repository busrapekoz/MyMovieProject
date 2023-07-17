namespace MyMovieProjectApi.Models
{
    public class Rating
    {
        public int id { get; set; }
        public int movieId { get; set; }
        public int? ratingValue { get; set; }
        public string note { get; set; }
    }
}
