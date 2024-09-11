
namespace MovieBringer.Core.Entities
{
    public class MovieListHistory:BaseEntity
    {
        public int MovieId { get; set; }
        public int MovieListId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
