namespace MyMovieProjectApi.Models
{
    public class MovieAddResponse : BaseResponse
    {
        public List<Movie> results { get; set; }
        public int page { get; set; }

    }
}
