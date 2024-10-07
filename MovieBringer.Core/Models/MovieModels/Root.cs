namespace MovieBringer.Core.Models.MovieModels
{
    public class Root
    {
        public Dates dates { get; set; }
        public int page { get; set; }
        public List<Movie> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }

    }

    public class Dates
    {
        public string maximum { get; set; }
        public string minimum { get; set; }
    }
}
