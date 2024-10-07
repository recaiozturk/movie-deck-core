namespace MovieBringer.Core.Entities
{
    public class VoteMovieList
    {
        public int Id { get; set; }
        public string VoteOwnerId { get; set; } = null!;
        public int ListId { get; set; }
        public decimal ListRank { get; set; }
    }
}
