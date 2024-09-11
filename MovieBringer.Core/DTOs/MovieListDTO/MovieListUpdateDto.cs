
namespace MovieBringer.Core.DTOs.MovieListDTO
{
    public class MovieListUpdateDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string ListOwner { get; set; } = null!;
        public double? ListRank { get; set; }
        public string ListName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPrivate { get; set; }
        public int MovieListType { get; set; }
    }
}
