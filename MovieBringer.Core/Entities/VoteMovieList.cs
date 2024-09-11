using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
