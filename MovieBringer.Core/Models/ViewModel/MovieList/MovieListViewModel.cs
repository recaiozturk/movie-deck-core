
using MovieBringer.Core.Models.MovieModels;
using System.ComponentModel.DataAnnotations;

namespace MovieBringer.Core.Models.ViewModel.MovieList
{
    public class MovieListViewModel
    {
        public int Id { get; set; }
        [Required]
        public string ListName { get; set; } = null!;
        public string? Description { get; set; }
        public int MovieListType { get; set; }
        public bool IsPrivate { get; set; }

        public List<ListEditMovieModel>? MoviesInlist { get; set; }
    }
}
