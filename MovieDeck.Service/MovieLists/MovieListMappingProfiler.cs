using AutoMapper;
using MovieDeck.Repository.MovieLists;
using MovieDeck.Service.MovieLists.Dtos;

namespace MovieDeck.Service.MovieLists
{
    public class MovieListMappingProfiler: Profile
    {
        public MovieListMappingProfiler()
        {
            CreateMap<MovieList, MovieListDto>().ReverseMap();
        }
    }
}
