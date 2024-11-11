using MovieDeck.Service.MovieLists.Dtos;
using MovieDeck.Service.Shared;

namespace MovieDeck.Service.MovieLists
{
    public interface IMovieListService
    {
        Task<ApiServiceResult<List<MovieListDto>>> GetListsByUserId(string userID);
    }
}
