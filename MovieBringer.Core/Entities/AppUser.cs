

using Microsoft.AspNetCore.Identity;

namespace MovieBringer.Core.Entities
{
    public class AppUser: IdentityUser
    {
        public string? Name { get; set; }

        public string? Surname { get; set; } 

        public string? ProfileImage { get; set; }
    }
}
