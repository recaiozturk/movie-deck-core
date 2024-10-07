using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieBringer.Core.DTOs.MovieListDTO;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Models.MovieModels;
using MovieBringer.Core.Models.ViewModel.MovieList;
using MovieBringer.Core.Services;
using MovieBringer.WebApp.Services.Abstract;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MovieBringer.WebApp.Controllers
{
    public class MovieListController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMovieListService _movieListService;
        private readonly IMovieListDTOService _movieListDtoService;
        private readonly IApiService _apiService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string baseUrl = "";

        public MovieListController(UserManager<AppUser> userManager, IApiService apiService, IConfiguration config, IMapper mapper, IMovieListService movieListService, IHttpContextAccessor httpContextAccessor, IMovieListDTOService movieListDtoService)
        {
            _userManager = userManager;
            _apiService = apiService;
            _config = config;
            baseUrl = _config.GetSection("AppSettings:WebUrl").Value;
            _mapper = mapper;
            _movieListService = movieListService;
            _httpContextAccessor = httpContextAccessor;
            _movieListDtoService = movieListDtoService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _movieListService.GetPublicMovieLists();
            ViewBag.UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (model == null)
            {
                TempData["message"] = "Error happened when reaching lists";
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public async Task<IActionResult> ListDetail(int listId)
        {
            ViewBag.listId = listId;
            ViewBag.UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);


            ViewBag.ListRank=await _movieListService.GetMovieListRank(listId);
            var moviesForListDetail = await _movieListService.GetMoviesForListDetail(listId);
            

            return View(moviesForListDetail);
        }

        [HttpGet]
        public async Task<JsonResult> MovieSearch(string value)
        {
            return Json(await _movieListService.MovieSearch(value));
        }

        [HttpPost]
        public async Task<JsonResult> AddMovieToList(int movieId, int listId)
        {
            return Json(await _movieListService.AddMovieToList(movieId, listId));           
        }

        public IActionResult CreateList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateList(MovieListViewModel model)
        {
            if (ModelState.IsValid)
            {
                MovieListCreateDto movieListCreateDto = _mapper.Map<MovieListCreateDto>(model);

                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    movieListCreateDto.UserId = user.Id;
                    movieListCreateDto.ListOwner = user.UserName;

                    var createListRequest = await _apiService.PostAsync($"{baseUrl}/api/MovieList", movieListCreateDto);

                    if (createListRequest.IsSuccess)
                    {
                        var jsonData = createListRequest.Data.ToString();
                        var movieListE = JsonConvert.DeserializeObject<MovieList>(jsonData);

                        TempData["message"] = "List Created Succesfully.You can add to movies for your list now";
                        TempData["messageType"] = "success";

                        return RedirectToAction("EditList", new { listId = movieListE.Id });
                    }
                    else
                    {
                        ModelState.Clear();
                        foreach (var err in createListRequest.Errors)
                        {
                            ModelState.AddModelError("CustomErrorKey", err);
                        }
                    }

                }
                else
                    return RedirectToAction("Index", "Home");

            }
            return View(model);
        }

        public async Task<IActionResult> EditList(int listId)
        {
            if (listId != 0)
            {
                var listRequest = await _apiService.GetAsync($"{_config.GetSection("AppSettings:WebUrl").Value}/api/MovieList/{listId}");

                if (listRequest.IsSuccess)
                {
                    var jsonData = listRequest.Data.ToString();
                    var movieListE = JsonConvert.DeserializeObject<MovieList>(jsonData);

                    var model = _mapper.Map<MovieListViewModel>(movieListE);

                    List<MovieListHistory> historyList = new List<MovieListHistory>();
                    List<ListEditMovieModel> listEditMovies = new List<ListEditMovieModel>();

                    var listMovieHistoriesRequest = await _apiService.GetAsync($"{_config.GetSection("AppSettings:WebUrl").Value}/api/MovieList/get-movieids?listId={listId}");

                    if (listMovieHistoriesRequest.IsSuccess)
                    {
                        var jsonData2 = listMovieHistoriesRequest.Data.ToString();
                        var movieListE2 = JsonConvert.DeserializeObject<List<MovieListHistory>>(jsonData2);
                        historyList = movieListE2;
                    }

                    listEditMovies = await _movieListService.GetEditListMovies(historyList);
                    model.MoviesInlist = listEditMovies;

                    return View(model);
                }

            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditList(MovieListViewModel modelPost)
        {
            if (ModelState.IsValid)
            {
                var listRequest = await _apiService.GetAsync($"{baseUrl}/api/MovieList/{modelPost.Id}");

                if (listRequest.IsSuccess)
                {
                    var jsonData = listRequest.Data.ToString();
                    var movieListE = JsonConvert.DeserializeObject<MovieList>(jsonData);
                    movieListE = _mapper.Map(modelPost, movieListE);


                    //apiRequest s
                    //var updateListPostRequest = await _apiService.PutAsync($"{baseUrl}/api/MovieList", movieListE);

                    var updateListPostRequest = await _movieListDtoService.UpdateAsync(movieListE);

                    //formdan gelen modeli güncelledik tekrardan getEdit list gerçekleştiriyoruz
                    var model = _mapper.Map<MovieListViewModel>(movieListE);

                    List<MovieListHistory> historyList = new List<MovieListHistory>();
                    List<ListEditMovieModel> listEditMovies = new List<ListEditMovieModel>();

                    var listMovieHistoriesRequest = await _apiService.GetAsync($"{baseUrl}/api/MovieList/get-movieids?listId={modelPost.Id}");

                    if (listMovieHistoriesRequest.IsSuccess)
                    {
                        var jsonData2 = listMovieHistoriesRequest.Data.ToString();
                        var movieListE2 = JsonConvert.DeserializeObject<List<MovieListHistory>>(jsonData2);
                        historyList = movieListE2;
                    }

                    listEditMovies = await _movieListService.GetEditListMovies(historyList);
                    model.MoviesInlist = listEditMovies;

                    TempData["message"] = "List Updated Succesfully.";
                    TempData["messageType"] = "success";

                    return View(model);

                }

                return NotFound();

            }
            return NotFound();
        }

        [HttpPost]
        public async Task<JsonResult> DeleteList(int listId)
        {
            return Json(await _movieListService.DeleteMovieList(listId));
        }

        [HttpPost]
        public async Task<JsonResult> RemoveMovieFromList(int movieListHistoryId, int movieListId)
        {
            return Json(await _movieListService.RemoveMovieFromList(movieListHistoryId, movieListId));
        }

        [HttpPost]
        public async Task<JsonResult> UpOrder(int movieListHistoryId,int listId)
        {
            return Json(await _movieListService.UpOrder(movieListHistoryId, listId));
        }

        [HttpPost]
        public async Task<JsonResult> DownOrder(int movieListHistoryId, int listId)
        {
            return Json(await _movieListService.DownOrder(movieListHistoryId, listId));
        }

        [HttpPost]
        public async Task<JsonResult> VoteList(int voteValue,int listId,string voteOwnerId)
        {
            
            return Json(await _movieListService.VoteMovieList(voteValue, listId, voteOwnerId));
        }

        [HttpPost]
        public async Task<JsonResult> VoteCheck(int voteValue, int listId, string voteOwnerId)
        {

            return Json(await _movieListService.CheckMovieListVotable(voteValue, listId, voteOwnerId));
        }
    }
}
