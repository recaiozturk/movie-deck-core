
using MovieBringer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBringer.Core.Repositories
{
    public interface IMovieListHistoryRepository:IGenericRepository<MovieListHistory>
    {
        Task<List<MovieListHistory>> GetMovieIdsByListId(int listId);

        Task<int> SetFirsDisplayOrder(int listId);

        Task<MovieListHistory> FindByDisplayOrder(int displayOrder,int listId);

        Task<int> GetHistoryCount(int listId);
        Task SyncDisplayOrder(int listId);

        Task DeleteList(int listId);
        Task<MovieListHistory> GetMovieHistoryWithListId(int movieHistoryId, int listId);
    }
}
