using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBringer.Core.DTOs;
using MovieBringer.Core.DTOs.MovieListDTO;
using MovieBringer.Core.DTOs.MovieListHistoryDTO;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Models.MovieModels;
using MovieBringer.Core.Services;
using MovieBringer.Service.Services;

namespace MovieBringer.WebApp.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieListController : ControllerBase
    {
        private readonly IMovieListDTOService _movieListDtoService;
        private readonly IMovieListHistoryDTOService _movlieListHistoryDtoService;

        public MovieListController(IMovieListDTOService movlieListDtoService, IMovieListHistoryDTOService movlieListHistoryDtoService)
        {
            _movieListDtoService = movlieListDtoService;
            _movlieListHistoryDtoService = movlieListHistoryDtoService;
        }

        [HttpGet("{id}")]
        public async Task<CustomResponseDto<MovieListDto>> Get(int id)
        {
            return await _movieListDtoService.GetByIdAsync(id);
        }

        [HttpGet("get-lists")]
        public async Task<CustomResponseDto<List<MovieListDto>>> GetListsByUserId(string userid)
        {
            return  await _movieListDtoService.GetListsByUserId(userid);
        }

        [HttpGet("get-movieids")]
        public async Task<CustomResponseDto<List<MovieListHistoryDto>>> GetMovieIdsByListId(int listId)
        {
            return await _movlieListHistoryDtoService.GetMovieIdsByListId(listId);
        }

        [HttpPost]
        public async Task<CustomResponseDto<MovieListDto>> Create(MovieListCreateDto movieListRequestModel)
        {
            return await _movieListDtoService.AddAsync(movieListRequestModel);      
        }

        [HttpPut]
        public async Task<CustomResponseDto<NoContentDto>> UpdateList(MovieListUpdateDto model)
        {
            return await _movieListDtoService.UpdateAsync(model);
        }

        [HttpDelete("delete-list")]
        public async Task<CustomResponseDto<NoContentDto>> DeleteList(int listId)
        {
            return await _movieListDtoService.DeleteMovieList(listId);
        }

        [HttpDelete("delete-movie-from-list")]
        public async Task<CustomResponseDto<NoContentDto>> DeleteMovieFormList(int movieHistoryId)
        {
            return await _movieListDtoService.RemoveMovieFromList(movieHistoryId);
        }

        [HttpGet("sync-display-orders")]
        public async Task<CustomResponseDto<NoContentDto>> SyncDisplayOrders(int listId)
        {
            return await _movlieListHistoryDtoService.SyncDisplayOrder(listId);
           
        }

        [HttpPost("create-movie-history")]
        public async Task<CustomResponseDto<MovieListHistory>> CreateMovieListHistory(MovieListHistoryCreateModel movieListHistoryModel)
        {
            return await _movieListDtoService.CreateMovieListHistory(movieListHistoryModel);
        }

        [HttpGet("up-movie-order")]
        public async Task<CustomResponseDto<NoContentDto>> MovieOrderUp(int movieListHistoryId,int listId)
        {
            return await _movieListDtoService.MovieOrderUp(movieListHistoryId, listId);
        }

        [HttpGet("down-movie-order")]
        public async Task<CustomResponseDto<NoContentDto>> MovieOrderDown(int movieListHistoryId, int listId)
        {
            return await _movieListDtoService.MovieOrderDown(movieListHistoryId, listId);
        }

        [HttpGet("vote-movielist")]
        public async Task<CustomResponseDto<NoContentDto>> VoteMovieList(int voteValue, int listId, string voteOwnerId)
        {
            return await _movieListDtoService.VoteMovieList( voteValue,  listId, voteOwnerId);
        }

        [HttpGet("vote-check")]
        public async Task<CustomResponseDto<VoteMovieList>> CheckMovieListVotable(int voteValue, int listId, string voteOwnerId)
        {
            return await _movieListDtoService.CheckMovieListVotable(voteValue, listId, voteOwnerId);
        }

        [HttpGet("get-listvote")]
        public async Task<CustomResponseDto<double>> GetMovieListVote(int listId)
        {
            return await _movieListDtoService.GetMovieListVote(listId);
        }
    }
}
