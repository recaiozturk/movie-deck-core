using Microsoft.EntityFrameworkCore;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Repositories;


namespace MovieBringer.Repository.Repositories
{
    public class MovieListRepository : GenericRepository<MovieList>, IMovieListRepository
    {

        public MovieListRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<MovieList>> GetListsByUserId(string userID)
        {
            return await _context.MovieLists.Where(l => l.UserId == userID).ToListAsync();
        }

        public async Task<int> GetMostPopulerListId()
        {
            var voteMoviesList = _context.VoteMovieLists;

            if (voteMoviesList.Any())
            {
                var highestAverageListID = voteMoviesList
                .GroupBy(vm => vm.ListId)
                .Select(g => new
                {
                    ListId = g.Key,
                    AverageRank = g.Average(vm => vm.ListRank)
                })
                .OrderByDescending(g => g.AverageRank)
                .FirstOrDefault().ListId;

                return highestAverageListID;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> GetLastAddedListId()
        {
            var movieLists = _context.MovieLists;
            if (movieLists.Any())
            {
                var lastAddedListID = _context.MovieLists
                    .OrderByDescending(vm => vm.CreatedDate)
                    .FirstOrDefault().Id;

                return lastAddedListID;
            }
            else
                return 0;

        }

        public async Task<List<MovieList>> GetPublicLists()
        {
            return await _context.MovieLists.Where(l => l.IsPrivate == false && _context.MovieListHistories.Any(h => h.MovieListId == l.Id)).ToListAsync();
        }

        public async Task<bool> IsListVoted(string userId, int listId)
        {
            return await _context.VoteMovieLists.AnyAsync(v => v.ListId == listId & v.VoteOwnerId == userId);
        }

        public async Task<VoteMovieList> GetVoteMovieList(string userId, int listId)
        {
            return await _context.VoteMovieLists.FirstOrDefaultAsync(v => v.ListId == listId & v.VoteOwnerId == userId);
        }

        public async Task CreteVoteListMovie(int voteValue, int listId, string voteOwnerId)
        {
            await _context.VoteMovieLists.AddAsync(new VoteMovieList { ListId = listId, ListRank = voteValue, VoteOwnerId = voteOwnerId });
        }

        public async Task UpdateVoteListMovie(int voteValue, int listId, string voteOwnerId)
        {
            var voteMovieList = await GetVoteMovieList(voteOwnerId, listId);
            voteMovieList.ListRank = voteValue;

            _context.VoteMovieLists.Update(voteMovieList);
        }

        public async Task<double> GetRankMovieList(int listId)
        {
            var anyRanked = _context.VoteMovieLists.Any(vm => vm.ListId == listId);

            if (anyRanked)
            {
                return (double)_context.VoteMovieLists.Where(vm => vm.ListId == listId)
                    .Average(vm => vm.ListRank);
            }
            else
            {
                return 0;
            }

        }
    }
}
