namespace MyMovieProjectApi.Models
{
    public class MovieListResponse : BaseResponse
    {
        public List<Movie> results { get; set; }
        public int page { get; set; }
    }
}
