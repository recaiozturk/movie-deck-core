using MovieBringer.Core.DTOs;
using MovieBringer.Core.Models.MovieModels;
using MovieBringer.WebApp.Services.Abstract;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using static MovieBringer.Core.Models.MovieModels.MovieImage;
using static MovieBringer.Core.Models.MovieModels.MovieVideo;

namespace MovieBringer.WebApp.Services.Concrate
{
    public class MovieService : IMovieService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public MovieService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("AppSettings:OMDBBaseUrl").Value);
        }
        public async Task<CustomResponseDto<List<Movie>>> GetMovies(string url, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<Root>(url);
            var movies = ResponseToMovieList(response);
            return CustomResponseDto<List<Movie>>.Success(200, movies);
        }

        public async Task<CustomResponseDto<RootCast>> GetMovieCast(int movieID, string token)
        {

            string url = $"movie/{movieID}/credits?language=en-US";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var cast = await _httpClient.GetFromJsonAsync<RootCast>(url);

            return CustomResponseDto<RootCast>.Success(200, cast);

        }

        public async Task<CustomResponseDto<MovieDetail>> GetMovieDetail(int movieId, string token)
        {
            string url = $"movie/{movieId}?language=en-US";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var movieDetail = await _httpClient.GetFromJsonAsync<MovieDetail>(url);
            return CustomResponseDto<MovieDetail>.Success(200, movieDetail);
        }

        public async Task<CustomResponseDto<RootImage>> GetMovieImages(int movieID, string token)
        {
            string url = $"movie/{movieID}/images";
            var movieImageRoot = await _httpClient.GetFromJsonAsync<RootImage>(url);
            return CustomResponseDto<RootImage>.Success(200, movieImageRoot);
        }

        public async Task<CustomResponseDto<List<Movie>>> GetMoviesBySearch(string url, string token, int movieCount)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<Root>(url);
            var movies = ResponseToMovieList(response);
            var moviesForSend = movies.Take(movieCount);

            return CustomResponseDto<List<Movie>>.Success(200, moviesForSend.ToList());

        }

        public async Task<CustomResponseDto<List<Movie>>> GetMoviesSmiler(int movieID, string token)
        {
            string url = $"movie/{movieID}/similar?language=en-US";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<Root>(url);
            var movies = ResponseToMovieList(response);
            return CustomResponseDto<List<Movie>>.Success(200, movies);

        }

        public async Task<CustomResponseDto<RootVideo>> GetMovieVideos(int movieID, string token)
        {
            string url = $"movie/{movieID}/videos?language=en-US";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var videos = await _httpClient.GetFromJsonAsync<RootVideo>(url);

            return CustomResponseDto<RootVideo>.Success(200, videos);

        }

        public List<Movie> ResponseToMovieList(Root response)
        {
            List<Movie> movies = new List<Movie>();
            for (int i = 0; i < response.results.Count; i++)
            {
                movies.Add(response.results[i]);
            }
            return movies;
        }
    }
}
