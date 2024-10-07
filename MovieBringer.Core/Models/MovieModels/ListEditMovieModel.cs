namespace MovieBringer.Core.Models.MovieModels
{
    public class ListEditMovieModel
    {
        public string? title { get; set; }
        public string? id { get; set; }
        public double vote_average { get; set; }
        public string? release_date { get; set; }
        public string? poster_path { get; set; }
        public int MovieHistoryId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
