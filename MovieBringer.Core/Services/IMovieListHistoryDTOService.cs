using MovieBringer.Core.DTOs;
using MovieBringer.Core.DTOs.MovieListHistoryDTO;
using MovieBringer.Core.Entities;

namespace MovieBringer.Core.Services
{
    public interface IMovieListHistoryDTOService : IDTOService<MovieListHistory, MovieListHistoryDto>
    {
        Task<CustomResponseDto<List<MovieListHistoryDto>>> GetMovieIdsByListId(int listId);

        Task<CustomResponseDto<int>> SetFirsDisplayOrder(int listId);

        Task<CustomResponseDto<MovieListHistoryDto>> FindByDisplayOrder(int displayOrder, int listId);

        Task<CustomResponseDto<int>> GetHistoryCount(int listId);
        Task<CustomResponseDto<NoContentDto>> SyncDisplayOrder(int listId);

        Task<CustomResponseDto<NoContentDto>> DeleteList(int listId);
    }
}
