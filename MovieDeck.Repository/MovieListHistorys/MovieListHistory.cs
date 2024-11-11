using MovieDeck.Repository.Shared.Entities;

namespace MovieDeck.Repository.MovieListHistorys
{
    public class MovieListHistory:BaseEntity
    {
        public int MovieId { get; set; }
        public int MovieListId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
