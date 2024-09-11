using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBringer.Core.Models.MovieModels
{
    public class ListDetailViewModel
    {
        public List<MovieBringer.Core.Models.MovieModels.MovieDetailModel>? MoviesInlist { get; set; }
        public string? ListOwner { get; set; }
        public string? ListOwnerId { get; set; }
        public string? ListName { get; set; }
        public int MovieListId { get; set; }

    }
}
