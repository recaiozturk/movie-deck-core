

using System.ComponentModel.DataAnnotations;

namespace MovieBringer.Core.Models.MovieModels
{
    public class MovieListHistoryCreateModel
    {

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int MovieListId { get; set; }
    }
}
