using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieDeck.Repository.MovieListHistorys;
using MovieDeck.Repository.MovieLists;
using MovieDeck.Repository.Shared.Entities;

namespace MovieDeck.Repository.Shared
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<AppUser,AppRole,Guid>(options)
    {
        public DbSet<MovieList> MovieLists { get; set; }
        public DbSet<MovieListHistory> MovieListHistories { get; set; }

        public DbSet<VoteMovieList> VoteMovieLists { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
    
}
