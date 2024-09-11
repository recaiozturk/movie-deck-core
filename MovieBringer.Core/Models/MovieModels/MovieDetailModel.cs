
using static MovieBringer.Core.Models.MovieModels.MovieImage;
using static MovieBringer.Core.Models.MovieModels.MovieVideo;

namespace MovieBringer.Core.Models.MovieModels
{
    public class MovieDetailModel
    {
        public MovieDetail? MovieDetail { get; set; }
        public List<Cast>? Casts { get; set; }

        public List<CREW>? Crews { get; set; }

        public List<MovieVideoResult>? MovieVideos { get; set; }

        public List<Movie>? MoviesSimilar { get; set; }

        public List<Backdrop>? BackdropImages { get; set; }
        public List<Poster>? PosterImages { get; set; }

    }
}
