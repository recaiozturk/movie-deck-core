

namespace MovieBringer.Core.DTOs.MovieListHistoryDTO
{
    public class MovieListHistoryUpdateDto
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int MovieListId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
