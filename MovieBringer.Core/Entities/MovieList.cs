using MovieBringer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBringer.Core.Entities
{
    public class MovieList:BaseEntity
    {
        public string UserId { get; set; } = null!;
        public string ListOwner { get; set; } = null!;
        public double? ListRank { get; set; }
        public string ListName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPrivate { get; set; }
        public int MovieListType { get; set; }
    }
}
