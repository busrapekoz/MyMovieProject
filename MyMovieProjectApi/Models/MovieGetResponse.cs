namespace MyMovieProjectApi.Models
{
    public class MovieGetResponse : BaseResponse
    {
        public Movie result { get; set; }
        public List<Rating> movieRating { get; set; }
        public double RatingAverage { get; set; }
    }
}
