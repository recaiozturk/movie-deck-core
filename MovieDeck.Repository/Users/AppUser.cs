using Microsoft.AspNetCore.Identity;
using MovieDeck.Repository.MovieLists;

namespace MovieDeck.Repository.Users
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string? ProfileImage { get; set; }
        public string? City { get; set; }
        public List<MovieList>? MovieLists { get; set; }
    }
}
