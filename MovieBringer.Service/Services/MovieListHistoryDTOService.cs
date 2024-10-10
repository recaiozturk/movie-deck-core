using AutoMapper;
using Microsoft.AspNetCore.Http;
using MovieBringer.Core.DTOs;
using MovieBringer.Core.DTOs.MovieListHistoryDTO;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Repositories;
using MovieBringer.Core.Services;
using MovieBringer.Core.UnitOfWorks;


namespace MovieBringer.Service.Services
{
    public class MovieListHistoryDTOService : DTOService<MovieListHistory, MovieListHistoryDto>, IMovieListHistoryDTOService
    {
        private readonly IMovieListHistoryRepository _movieListHistoryRepository;
        private readonly IMovieListRepository _movieListRepository;
        public MovieListHistoryDTOService(IMapper mapper, IUnitOfWork unitOfWork, IGenericRepository<MovieListHistory> repository, IMovieListHistoryRepository movieListHistoryRepository) : base(mapper, unitOfWork, repository)
        {
            _movieListHistoryRepository = movieListHistoryRepository;
        }
        public async Task<CustomResponseDto<NoContentDto>> DeleteList(int listId)
        {
            await _movieListHistoryRepository.DeleteList(listId);
            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);

        }

        public async Task<CustomResponseDto<MovieListHistoryDto>> FindByDisplayOrder(int displayOrder,int listId)
        {
            var movieListHistory=await _movieListHistoryRepository.FindByDisplayOrder(displayOrder, listId);
            var movieListDto=_mapper.Map<MovieListHistoryDto>(movieListHistory);
            return  CustomResponseDto<MovieListHistoryDto>.Success(200,movieListDto);
        }

        public async Task<CustomResponseDto<int>> GetHistoryCount(int listId)
        {
            var historyCount=await _movieListHistoryRepository.GetHistoryCount(listId);
            return  CustomResponseDto<int>.Success(200, historyCount);
        }

        public async Task<CustomResponseDto<List<MovieListHistoryDto>>> GetMovieIdsByListId(int listId)
        {
            var movieIds= await _movieListHistoryRepository.GetMovieIdsByListId(listId);
            var movieIdsDto=_mapper.Map<List<MovieListHistoryDto>>(movieIds);
            return  CustomResponseDto<List<MovieListHistoryDto>>.Success(200,movieIdsDto);
        }

        public async Task<CustomResponseDto<int>> SetFirsDisplayOrder(int listId)
        {
            var listCount = await _movieListHistoryRepository.SetFirsDisplayOrder(listId);
            listCount = listCount + 1;
            return CustomResponseDto<int>.Success(200, listCount);
        }

        public async Task<CustomResponseDto<NoContentDto>> SyncDisplayOrder(int listId)
        {
            await _movieListHistoryRepository.SyncDisplayOrder(listId);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }
    }
}
