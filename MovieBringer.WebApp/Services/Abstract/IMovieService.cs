using MovieBringer.Core.DTOs;
using MovieBringer.Core.Models.MovieModels;
using static MovieBringer.Core.Models.MovieModels.MovieImage;
using static MovieBringer.Core.Models.MovieModels.MovieVideo;

namespace MovieBringer.WebApp.Services.Abstract
{
    public interface IMovieService
    {
        Task<CustomResponseDto<List<Movie>>> GetMovies(string apiUrl, string token);

        Task<CustomResponseDto<MovieDetail>> GetMovieDetail(int movieId, string token);

        Task<CustomResponseDto<RootCast>> GetMovieCast(int movieID, string token);
        Task<CustomResponseDto<RootVideo>> GetMovieVideos(int movieID, string token);

        Task<CustomResponseDto<List<Movie>>> GetMoviesSmiler(int movieID, string token);
        Task<CustomResponseDto<RootImage>> GetMovieImages(int movieID, string token);
        Task<CustomResponseDto<List<Movie>>> GetMoviesBySearch(string apiUrl, string token, int movieCount);

        List<Movie> ResponseToMovieList(Root response);
    }
}
