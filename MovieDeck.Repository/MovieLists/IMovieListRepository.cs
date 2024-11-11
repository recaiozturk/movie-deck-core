using MovieDeck.Repository.Shared;
using MovieDeck.Repository.Shared.Entities;

namespace MovieDeck.Repository.MovieLists
{
    public interface IMovieListRepository: IGenericRepository<MovieList>
    {
        Task<List<MovieList>> GetListsByUserId(string userID);
        Task<List<MovieList>> GetPublicLists();
        Task<bool> IsListVoted(string userId, int listId);
        Task CreteVoteListMovie(int voteValue, int listId, string voteOwnerId);
        Task UpdateVoteListMovie(int voteValue, int listId, string voteOwnerId);
        Task<VoteMovieList> GetVoteMovieList(string userId, int listId);
        Task<double> GetRankMovieList(int listId);
        Task<int> GetMostPopulerListId();
        Task<int> GetLastAddedListId();
    }
}
