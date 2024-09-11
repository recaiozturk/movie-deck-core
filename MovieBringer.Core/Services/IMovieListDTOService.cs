using MovieBringer.Core.DTOs;
using MovieBringer.Core.DTOs.MovieListDTO;
using MovieBringer.Core.DTOs.UserDTO;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Models.MovieModels;


namespace MovieBringer.Core.Services
{
    public interface IMovieListDTOService:IDTOService<MovieList,MovieListDto>
    {
        Task<CustomResponseDto<List<MovieListDto>>> GetListsByUserId(string userID);

        Task<CustomResponseDto<List<MovieListDto>>> GetPublicLists();

        //ovverride 
        Task<CustomResponseDto<NoContentDto>> UpdateAsync(MovieListUpdateDto dto);

        Task<CustomResponseDto<NoContentDto>> UpdateAsync(MovieList movieList);

        //ovverride 
        Task<CustomResponseDto<MovieListDto>> AddAsync(MovieListCreateDto dto);

        Task<CustomResponseDto<MovieListHistory>> CreateMovieListHistory(MovieListHistoryCreateModel movieListHistoryModel);

        Task<CustomResponseDto<NoContentDto>> DeleteMovieList(int listId);
        Task<CustomResponseDto<NoContentDto>> RemoveMovieFromList(int movieHistoryId);
        Task<CustomResponseDto<NoContentDto>> MovieOrderUp(int movieListHistoryId, int listId);
        Task<CustomResponseDto<NoContentDto>> MovieOrderDown(int movieListHistoryId, int listId);

        Task<CustomResponseDto<NoContentDto>> VoteMovieList(int voteValue, int listId, string voteOwnerId);

        Task<CustomResponseDto<VoteMovieList>> CheckMovieListVotable(int voteValue, int listId, string voteOwnerId);
        Task<CustomResponseDto<double>> GetMovieListVote(int listId);
        Task<CustomResponseDto<int>> GetMostPopulerListId();

        Task<CustomResponseDto<int>> GetLastAddedListId();
    }
}
