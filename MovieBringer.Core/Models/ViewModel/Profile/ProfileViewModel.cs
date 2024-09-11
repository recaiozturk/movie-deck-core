
using System.ComponentModel.DataAnnotations;




namespace MovieBringer.Core.Models.ViewModel.Profile
{
    public class ProfileViewModel
    {
        public string? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        public List<Entities.MovieList>? MovieLists { get; set; }

    }
}
