namespace MovieBringer.Core.Models.MovieModels
{
    public class MovieListDetailModel
    {
        public int MovieListId { get; set; }
        public string ListName { get; set; }
        public double MovieListRank { get; set; }
        public string? MovieListOwner { get; set; }
        public string? MovieListOwnerId { get; set; }
        public List<ListEditMovieModel>? MoviesInlist { get; set; }
    }

    public class MovieListDetailModel2
    {
        public int MovieListId { get; set; }
        public string? MovieListOwner { get; set; }
        public List<MovieDetail>? MoviesInlist { get; set; }
    }
}
