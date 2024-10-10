using Microsoft.AspNetCore.Mvc;
using MovieBringer.Core.Models.MovieModels;
using MovieBringer.Core.Models.ViewModel.Home;
using MovieBringer.WebApp.Services.Abstract;
using static MovieBringer.Core.Models.MovieModels.MovieVideo;

namespace MovieBringer.WebApp.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IProfileWebService _profileService;
        private readonly IConfiguration _configuration;
        private string OMDBApiKey;

        public MovieController(IMovieService movieService, IConfiguration configuration, IProfileWebService profileService)
        {
            _movieService = movieService;
            _configuration = configuration;
            OMDBApiKey = _configuration.GetSection("AppSettings:OMDBToken").Value;
            _profileService = profileService;
        }
        public async Task<IActionResult> Detail(int id)
        {
            MovieDetailModel model = new MovieDetailModel();

            //movie detail
            var movieDetailRequest = await _movieService.GetMovieDetail(id, OMDBApiKey);
            model.MovieDetail=movieDetailRequest.Data;

            //cast
            var castRequest = await _movieService.GetMovieCast(id, OMDBApiKey);
            var rootCast = castRequest.Data;
            model.Casts = rootCast.cast;
            model.Crews = rootCast.crew;

            //movie trailers,videos
            var videoRequest = await _movieService.GetMovieVideos(id, OMDBApiKey);
            RootVideo videoResult = videoRequest.Data;
            var videos = videoResult.results.ToList();
            model.MovieVideos = videos;

            //smiler movies
            var smilerMoviesRequest = await _movieService.GetMoviesSmiler(id, OMDBApiKey);
            model.MoviesSimilar = smilerMoviesRequest.Data;

            //movie images
            var movieImageRequest = await _movieService.GetMovieImages(id, OMDBApiKey);
            var data = movieImageRequest.Data;
            model.PosterImages = data.posters.ToList();
            model.BackdropImages = data.backdrops.ToList();

            return View(model);
        }

        public async Task<IActionResult> Movies()
        {
            HomePageViewModel model = new HomePageViewModel();

            //trending movies
            var trendingRequst = await _movieService.GetMovies("trending/movie/week?language=en-US", OMDBApiKey);
            model.TrendingInWeeks = (List<Movie>?)trendingRequst.Data;

            //now_playing movies
            var nowPlayingRequst = await _movieService.GetMovies("movie/now_playing?language=en-US&page=1", OMDBApiKey);
            model.NowPlayings = (List<Movie>?)nowPlayingRequst.Data;

            //upcoming movies
            var upcomingRequst = await _movieService.GetMovies("movie/upcoming?language=en-US&page=1", OMDBApiKey);
            model.UpComing = (List<Movie>?)upcomingRequst.Data;
            //populer movies
            var populerRequst = await _movieService.GetMovies("movie/popular?language=en-US&page=1", OMDBApiKey);
            model.Popular = (List<Movie>?)populerRequst.Data;

            //top_rated movies
            var topRatedRequst = await _movieService.GetMovies("movie/top_rated?language=en-US&page=1", OMDBApiKey);
            model.TopRated = (List<Movie>?)topRatedRequst.Data;

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetUserMovieLists(string userId)
        {
            var profileModel = await _profileService.GetProfileModel(userId);

            return Json(profileModel.MovieLists);
        }
    }
}
