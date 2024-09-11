
namespace MovieBringer.Core.Models.MovieModels
{
    public class MovieDetail
    {
        public bool adult  { get; set; }
        public string backdrop_path { get; set; }
        public BelongsToCollection belongs_to_collection { get; set; }
        public long budget { get; set; }
        public Genres[] genres { get; set; }
        public Uri Homepage { get; set; }
        public int id { get; set; }
        public string imdb_id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public ProductionCompanies[] production_companies { get; set; }
        public ProductionCountries[] production_countries { get; set; }

        public string ProductionCountriesSTR =>  "Not Found";
        public DateTimeOffset release_date { get; set; }
        public long revenue { get; set; }
        public long runtime { get; set; }
        public SpokenLanguages[] spoken_languages { get; set; }
        public string status { get; set; }
        public string tagline { get; set; }
        public string title { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public long vote_count { get; set; }

    }

    public class BelongsToCollection
    {
        public int id { get; set; }
        public string name { get; set; }
        public string poster_path { get; set; }
        public string backdrop_path { get; set; }
    }

    public class Genres
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ProductionCompanies
    {
        public int id { get; set; }
        public string logo_path { get; set; }
        public string name { get; set; }
        public string origin_country { get; set; }
    }

    public class ProductionCountries
    {
        public string iso_3166_1 { get; set; }
        public string name { get; set; }
    }

    public class SpokenLanguages
    {
        public string english_name { get; set; }
        public string iso_639_1 { get; set; }
        public string name { get; set; }
    }
}
