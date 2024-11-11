using Microsoft.AspNetCore.Identity;

namespace MovieDeck.Repository.Shared.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? ProfileImage { get; set; }
    }
}
