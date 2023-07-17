namespace MyMovieProjectApi.Models
{
    public class MovieRatingResponse : BaseResponse
    {
        public List<Rating> movieRating { get; set; }
    }
}
