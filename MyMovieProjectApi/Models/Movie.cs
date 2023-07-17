namespace MyMovieProjectApi.Models
{
    public class Movie
    {
        public int id { get; set; }
        public string title { get; set; }
        public string overview { get; set; }
        public DateTime release_date { get; set; }
    }
}
