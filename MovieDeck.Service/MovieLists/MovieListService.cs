using AutoMapper;
using MovieBringer.Repository.Repositories;
using MovieDeck.Service.MovieLists.Dtos;
using MovieDeck.Service.Shared;
using System.Net;

namespace MovieDeck.Service.MovieLists
{
    public class MovieListService(MovieListRepository _movieListRepository,Mapper _mapper) : IMovieListService
    {
        public async Task<ApiServiceResult<List<MovieListDto>>> GetListsByUserId(string userID)
        {
            var moviLists = await _movieListRepository.GetListsByUserId(userID);
            var moviListsDto = _mapper.Map<List<MovieListDto>>(moviLists);
            return ApiServiceResult<List<MovieListDto>>.Success(moviListsDto,HttpStatusCode.OK);
        }
    }
}
