using Microsoft.AspNetCore.Mvc;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Models.MovieModels;
using MovieBringer.WebApp.Models;

namespace MovieBringer.WebApp.Services.Abstract
{
    public interface IMovieListService
    {
        Task<List<ListEditMovieModel>> GetEditListMovies(List<MovieListHistory> historyList);
        Task<ListDetailViewModel> GetMoviesForListDetail(int listId);
        Task<CustomJsonModel> MovieSearch(string value);

        Task<CustomJsonModel> AddMovieToList(int movieId, int listId);

        Task<CustomJsonModel> DeleteMovieList(int listId);

        Task<CustomJsonModel> RemoveMovieFromList(int movieListHistoryId, int movieListId);

        Task<bool> SyncDisplayOrder(int listId);
        Task<CustomJsonModel> UpOrder(int movieListHistoryId,int listId);
        Task<CustomJsonModel> DownOrder(int movieListHistoryId, int listId);

        Task<List<MovieListDetailModel>> GetPublicMovieLists();

        Task<CustomJsonModel> VoteMovieList(int voteValue, int listId, string voteOwnerId);

        Task<CustomJsonModel> CheckMovieListVotable(int voteValue, int listId, string voteOwnerId);
        Task<double> GetMovieListRank(int listId);

    }
}
