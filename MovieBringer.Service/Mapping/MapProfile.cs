using AutoMapper;
using MovieBringer.Core.DTOs.MovieListDTO;
using MovieBringer.Core.DTOs.MovieListHistoryDTO;
using MovieBringer.Core.DTOs.UserDTO;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Models.MovieModels;
using MovieBringer.Core.Models.ViewModel.Account;
using MovieBringer.Core.Models.ViewModel.MovieList;
using MovieBringer.Core.Models.ViewModel.Profile;


namespace MovieBringer.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<UserDTO,AppUser>().ReverseMap();
            CreateMap<UserCreateDto, AppUser>().ReverseMap();
            CreateMap<UserUpdateDto, AppUser>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ProfileViewModel, AppUser>().ReverseMap();

            CreateMap<RegisterViewModel, AppUser>().ReverseMap();
            

            CreateMap<MovieList, MovieListDto>().ReverseMap();
            CreateMap<MovieListUpdateDto, MovieList>().ReverseMap(); 
            CreateMap<MovieListCreateDto, MovieList>().ReverseMap(); 

            CreateMap<MovieListCreateDto, MovieListViewModel>().ReverseMap();
            CreateMap<MovieList, MovieListViewModel>().ReverseMap();

            CreateMap<MovieListHistory, MovieListHistoryDto>().ReverseMap();
            CreateMap<MovieListHistory, MovieListHistoryCreateModel>().ReverseMap();
            CreateMap<MovieListHistoryUpdateDto, MovieListHistoryDto>().ReverseMap(); 
            CreateMap<MovieListHistoryCreateDto, MovieListHistoryDto>().ReverseMap();
            CreateMap<ListEditMovieModel, MovieDetail>().ReverseMap();



        }
    }
}
