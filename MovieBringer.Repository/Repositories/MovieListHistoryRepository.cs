using Microsoft.EntityFrameworkCore;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Repositories;


namespace MovieBringer.Repository.Repositories
{
    public class MovieListHistoryRepository : GenericRepository<MovieListHistory>, IMovieListHistoryRepository
    {
        public MovieListHistoryRepository(AppDbContext context) : base(context)
        {

        }

        public async Task DeleteList(int listId)
        {

            var voteMoviesToDelete = _context.VoteMovieLists.Where(v => v.ListId == listId).ToList();
            if (voteMoviesToDelete.Any())
                _context.VoteMovieLists.RemoveRange(voteMoviesToDelete);


            var movieHistories = await _context.MovieListHistories.Where(h => h.MovieListId == listId).ToListAsync();

            foreach (var movie in movieHistories)
            {
                _context.MovieListHistories.Remove(movie);
            }

            var list = await _context.MovieLists.FirstOrDefaultAsync(h => h.Id == listId);

            if (list != null)
                _context.MovieLists.Remove(list);

            await _context.SaveChangesAsync();
        }

        public async Task<MovieListHistory> FindByDisplayOrder(int displayOrder,int listId)
        {
            return await _context.MovieListHistories.FirstOrDefaultAsync(h => h.DisplayOrder == displayOrder && h.MovieListId==listId);
        }

        public async Task<int> GetHistoryCount(int listId)
        {
            return await _context.MovieListHistories.Where(h => h.MovieListId == listId).CountAsync();
        }

        public async Task<List<MovieListHistory>> GetMovieIdsByListId(int listId)
        {
            return await _context.MovieListHistories.Where(h => h.MovieListId == listId).OrderBy(h => h.DisplayOrder).ToListAsync();
        }

        public async Task<int> SetFirsDisplayOrder(int listId)
        {
            var listCount = await _context.MovieListHistories.Where(h => h.MovieListId == listId).CountAsync();
            return listCount + 1;

        }

        public async Task SyncDisplayOrder(int listId)
        {
            var movieHistories = await _context.MovieListHistories.OrderBy(h => h.DisplayOrder).Where(h => h.MovieListId == listId).ToListAsync();

            for (int i = 0; i < movieHistories.Count; i++)
            {
                movieHistories[i].DisplayOrder = i + 1;
            }
        }

        public async  Task<MovieListHistory> GetMovieHistoryWithListId(int movieHistoryId,int listId)
        {
            return await _context.MovieListHistories.Where(x=>x.Id== movieHistoryId && x.MovieListId==listId).FirstOrDefaultAsync();
        }
    }
}
