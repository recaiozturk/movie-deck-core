using Microsoft.AspNetCore.Mvc;
using MovieBringer.Core.Models.MovieModels;
using MovieBringer.Core.Models.ViewModel.Home;
using MovieBringer.Core.Services;
using MovieBringer.WebApp.Services.Abstract;

namespace MovieBringer.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IMovieListService _movieListService;
        private readonly IMovieListDTOService _movieListDTOService;
        private readonly IConfiguration _configuration;
        private string OMDBApiKey;

        public HomeController(IMovieService movieService, IConfiguration configuration, IMovieListService movieListService, IMovieListDTOService movieListDTOService)
        {
            _movieService = movieService;
            _configuration = configuration;
            OMDBApiKey = _configuration.GetSection("AppSettings:OMDBToken").Value;
            _movieListService = movieListService;
            _movieListDTOService = movieListDTOService;
        }
        public async Task<IActionResult> Index()
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

            //dynamic mostPopulerList 
            var mostRankedMovieListId = await _movieListDTOService.GetMostPopulerListId();
            if (mostRankedMovieListId.Data != 0)
                model.MostPopulerList = await _movieListService.GetMoviesForListDetail(mostRankedMovieListId.Data);
            else model.MostPopulerList = null;

            //dynamic lastAddadList
            var lastAddedListıd = await _movieListDTOService.GetLastAddedListId();
            if (lastAddedListıd.Data != 0)
                model.LastAddedList = await _movieListService.GetMoviesForListDetail(lastAddedListıd.Data);
            else model.LastAddedList = null;

            return View(model);
        }

        public async Task<IActionResult> Error()
        {
            return View();
        }
    }
}
