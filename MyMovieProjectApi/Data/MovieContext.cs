using Microsoft.EntityFrameworkCore;
using MyMovieProjectApi.Models;

namespace MyMovieProjectApi.Data
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<User> User { get; set; }

        public MovieContext(DbContextOptions<MovieContext> context):base(context)
        {
            
        }
    }
}
