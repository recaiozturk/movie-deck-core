using AutoMapper;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Models.MovieModels;
using MovieBringer.Core.Services;
using MovieBringer.WebApp.Models;
using MovieBringer.WebApp.Services.Abstract;
using MovieBringer.WebApp.Util.Abstract;
using Newtonsoft.Json;
using static MovieBringer.Core.Models.MovieModels.MovieImage;
using static MovieBringer.Core.Models.MovieModels.MovieVideo;

namespace MovieBringer.WebApp.Services.Concrate
{
    public class MovieListService: IMovieListService
    {
        private readonly IConfiguration _config;
        private readonly IMovieService _movieService;
        private readonly IApiService _apiService;
        private readonly IMovieListDTOService _movieListDtoService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private string baseUrl = "";

        public MovieListService(IConfiguration config, IMovieService movieService, IMapper mapper, IMovieListDTOService movieListDtoService, IApiService apiService, IEmailSender emailSender)
        {
            _config = config;
            _movieService = movieService;
            _mapper = mapper;
            _movieListDtoService = movieListDtoService;
            _apiService = apiService;
            baseUrl = _config.GetSection("AppSettings:WebUrl").Value;
            _emailSender = emailSender;
        }

        public async Task<List<ListEditMovieModel>> GetEditListMovies(List<MovieListHistory> historyList)
        {
            List<ListEditMovieModel> listEditMovieModels = new List<ListEditMovieModel>();

            foreach (var movie in historyList)
            {

                if (movie.MovieId != 0)
                {
                    var movieRequest = await _movieService.GetMovieDetail(movie.MovieId, _config.GetSection("AppSettings:OMDBToken").Value);

                    if (movieRequest.IsSuccess)
                    {
                        var editMovieModel = _mapper.Map<ListEditMovieModel>((MovieDetail)movieRequest.Data);
                        editMovieModel.MovieHistoryId = movie.Id;
                        editMovieModel.DisplayOrder = movie.DisplayOrder;
                        listEditMovieModels.Add(editMovieModel);
                    }
                }
                
            }
            return listEditMovieModels;
        }
        public async Task<ListDetailViewModel> GetMoviesForListDetail(int listId)
        {
            ListDetailViewModel viewModel= new ListDetailViewModel();
            var listRequest = await _movieListDtoService.GetByIdAsync(listId);
            List<MovieDetailModel> moviesForListDetail = new List<MovieDetailModel>();
            if (listRequest.IsSuccess)
            {
                MovieListDetailModel2 model = new MovieListDetailModel2();

                var listMovieHistoriesRequest = await _apiService.GetAsync($"{baseUrl}/api/MovieList/get-movieids?listId={listRequest.Data.Id}");
                viewModel.ListOwner = listRequest.Data.ListOwner;
                viewModel.ListOwnerId = listRequest.Data.UserId;
                viewModel.ListName = listRequest.Data.ListName;
                viewModel.MovieListId = listRequest.Data.Id;

                if (listMovieHistoriesRequest.IsSuccess)
                {
                    var jsonData2 = listMovieHistoriesRequest.Data.ToString();
                    var movieListE2 = JsonConvert.DeserializeObject<List<MovieListHistory>>(jsonData2);

                    foreach (var movieHis in movieListE2)
                    {
                        MovieDetailModel modelMD = new MovieDetailModel();

                        if (movieHis.MovieId != 0)
                        {
                            //movie detail
                            var movieRequest = await _movieService.GetMovieDetail(movieHis.MovieId, _config.GetSection("AppSettings:OMDBToken").Value);

                            if (movieRequest.IsSuccess)
                                modelMD.MovieDetail = (MovieDetail)movieRequest.Data;
                            else
                                modelMD.MovieDetail = null;
                            //cast
                            var castRequest = await _movieService.GetMovieCast(movieHis.MovieId, _config.GetSection("AppSettings:OMDBToken").Value);
                            if (castRequest.IsSuccess)
                            {
                                var rootCast = (RootCast)castRequest.Data;
                                modelMD.Casts = rootCast.cast;
                                modelMD.Crews = rootCast.crew;
                            }
                            else
                            {
                                modelMD.Casts = null;
                                modelMD.Crews = null;
                            }
                            //movie trailers,videos
                            var videoRequest = await _movieService.GetMovieVideos(movieHis.MovieId, _config.GetSection("AppSettings:OMDBToken").Value);
                            if (videoRequest.IsSuccess)
                            {
                                RootVideo videoResult = videoRequest.Data as RootVideo;
                                var videos = videoResult.results.ToList();
                                modelMD.MovieVideos = videos;
                            }
                            else
                                modelMD.MovieVideos = null;

                            //movie images
                            var movieImageRequest = await _movieService.GetMovieImages(movieHis.MovieId, _config.GetSection("AppSettings:OMDBToken").Value);

                            if (movieImageRequest.IsSuccess)
                            {
                                var data = (RootImage)movieImageRequest.Data;
                                modelMD.BackdropImages = data.backdrops.ToList();
                            }
                            moviesForListDetail.Add(modelMD);
                        }                      
                    }
                }
                else
                {
                    moviesForListDetail = null;
                }
            }
            viewModel.MoviesInlist = moviesForListDetail;
            return viewModel;
        }

        public async Task<CustomJsonModel> MovieSearch(string value)
        {
            var url = $"https://api.themoviedb.org/3/search/movie?query={value}&include_adult=false&language=en-US&page=1";
            var searchedMovies = await _movieService.GetMoviesBySearch(url, _config.GetSection("AppSettings:OMDBToken").Value, 5);

            if (searchedMovies.IsSuccess)
                return new CustomJsonModel { Data = searchedMovies.Data, IsValid = true };
            else
                return new CustomJsonModel { IsValid = false, ErrorMessages = new List<string> { "Error Happened" } };
        }

        public async Task<CustomJsonModel> AddMovieToList(int movieId, int listId)
        {

            if (movieId == 0)
            {
                return new CustomJsonModel
                {
                    IsValid = false,
                    ErrorMessage = "You didn't choose a movie"
                };
            }

            MovieListHistoryCreateModel model = new MovieListHistoryCreateModel();
            model.MovieId = movieId;
            model.MovieListId = listId;

            var createListHisRequest = await _apiService.PostAsync($"{baseUrl}/api/MovieList/create-movie-history", model);

            var jsonMovieHistory = createListHisRequest.Data.ToString();
            var movieHistory = JsonConvert.DeserializeObject<MovieListHistory>(jsonMovieHistory);


            if (createListHisRequest != null && createListHisRequest.IsSuccess)
            {
                var movieMovieRequest = await _movieService.GetMovieDetail(movieId, _config.GetSection("AppSettings:OMDBToken").Value);

                var editMovieModel = _mapper.Map<ListEditMovieModel>((MovieDetail)movieMovieRequest.Data);
                editMovieModel.MovieHistoryId = movieHistory.Id;
                editMovieModel.DisplayOrder = movieHistory.DisplayOrder;

                if (movieMovieRequest.IsSuccess)
                {
                    return new CustomJsonModel
                    {
                        
                        IsValid = true,
                        Data = editMovieModel,
                        SuccessMessage = "Added Successfuly"
                    };
                }
                else
                    return new CustomJsonModel
                    {
                        IsValid = false,
                        ErrorMessages = new List<string> { "Error Happened" }
                    };
            }

            else
                return new CustomJsonModel
                {
                    IsValid = false,
                    ErrorMessages = new List<string> { "Error Happened" }
                };
        }

        public async Task<CustomJsonModel> DeleteMovieList(int listId)
        {
            //api Request
            //var deleteListRequest = await _apiService.DeleteAsync($"{baseUrl}/api/MovieList/delete-list?listId={listId}");

            var deleteListRequest = await _movieListDtoService.DeleteMovieList(listId);

            if (deleteListRequest.IsSuccess)
            {
                return new CustomJsonModel { IsValid = true, SuccessMessage = deleteListRequest.Message };
            }
            else
            {
                return new CustomJsonModel { IsValid = false, ErrorMessage = deleteListRequest.Errors[0] };
            }
            
        }

        public async Task<CustomJsonModel> RemoveMovieFromList(int movieListHistoryId, int movieListId)
        {
            //api Request
            //var deleteHistoryRequest = await _apiService.DeleteAsync($"{baseUrl}/api/MovieList/delete-movie-from-list?movieHistoryId={movieListHistoryId}");

            var deleteHistoryRequest = await _movieListDtoService.RemoveMovieFromList(movieListHistoryId);

            if (deleteHistoryRequest.IsSuccess)
            {
                var isSync = await SyncDisplayOrder(movieListId);

                if (isSync)
                {
                    return new CustomJsonModel { IsValid = true,Data= deleteHistoryRequest.Data ,SuccessMessage = "Removed Successfuly" };
                }
                else
                {
                    return new CustomJsonModel { IsValid = false, ErrorMessages = new List<string> { "Error happened While Orders Sycning" }};
                }
            }
            else
                return new CustomJsonModel { IsValid = false, ErrorMessages = deleteHistoryRequest.Errors };
        }
        public async Task<bool> SyncDisplayOrder(int listId)
        {
            var syncRequest = await _apiService.GetAsync($"{baseUrl}/api/MovieList/sync-display-orders?listId={listId}");

            if (syncRequest.IsSuccess)
                return true;
            else
                return false;
        }

        public async Task<CustomJsonModel> UpOrder(int movieListHistoryId,int listId)
        {
            var uporderRequest = await _apiService.GetAsync($"{baseUrl}/api/MovieList/up-movie-order?movieListHistoryId={movieListHistoryId}&listId={listId}");

            if (uporderRequest.IsSuccess)
                return new CustomJsonModel { IsValid = true, SuccessMessage=uporderRequest.Message };
            else
                return new CustomJsonModel { IsValid = false, ErrorMessage = uporderRequest.Errors[0] ?? "Error Happened" };
        }

        public async Task<CustomJsonModel> DownOrder(int movieListHistoryId, int listId)
        {
            var uporderRequest = await _apiService.GetAsync($"{baseUrl}/api/MovieList/down-movie-order?movieListHistoryId={movieListHistoryId}&listId={listId}");

            if (uporderRequest.IsSuccess)
                return new CustomJsonModel { IsValid = true, SuccessMessage = uporderRequest.Message };
            else
                return new CustomJsonModel { IsValid = false, ErrorMessage = uporderRequest.Errors[0] ?? "Error Happened" };
        }


        public async Task<List<MovieListDetailModel>> GetPublicMovieLists()
        {

            MovieListDetailModel model2 = new MovieListDetailModel();
            List<MovieListDetailModel> modelLists = new List<MovieListDetailModel>();

            var listsRequest = await _movieListDtoService.GetPublicLists();

            foreach (var list in listsRequest.Data)
            {
                MovieListDetailModel model = new MovieListDetailModel();
                model.MovieListId = list.Id;
                model.ListName = list.ListName;
                model.MovieListOwner = list.ListOwner;
                model.MovieListOwnerId = list.UserId;
                model.MovieListRank = await GetMovieListRank(list.Id);

                List<MovieListHistory> historyList = new List<MovieListHistory>();

                var listMovieHistoriesRequest = await _apiService.GetAsync($"{baseUrl}/api/MovieList/get-movieids?listId={list.Id}");

                if (listMovieHistoriesRequest.IsSuccess)
                {
                    var jsonData2 = listMovieHistoriesRequest.Data.ToString();
                    var movieListE2 = JsonConvert.DeserializeObject<List<MovieListHistory>>(jsonData2);
                    historyList = movieListE2;
                    model.MoviesInlist = await GetEditListMovies(historyList);
                    modelLists.Add(model);
                }
                else
                {
                    modelLists = null;
                }
            }
            return modelLists;
        }

        public async Task<CustomJsonModel> VoteMovieList(int voteValue, int listId, string voteOwnerId)
        {
            if(voteValue<=0 || listId <= 0 ||  String.IsNullOrEmpty(voteOwnerId))
                return new CustomJsonModel { IsValid = false, ErrorMessage =  "Error Happened" };

            var voteRequest=await _apiService.GetAsync($"{baseUrl}/api/MovieList/vote-movielist?voteValue={voteValue}&listId={listId}&voteOwnerId={voteOwnerId}");

            if (voteRequest.IsSuccess)
                return new CustomJsonModel { IsValid = true, SuccessMessage = voteRequest.Message };
            else
                return new CustomJsonModel { IsValid = false, ErrorMessage = voteRequest.Message };
        }

        public async Task<CustomJsonModel> CheckMovieListVotable(int voteValue, int listId, string voteOwnerId)
        {
            var voteRequest = await _apiService.GetAsync($"{baseUrl}/api/MovieList/vote-check?voteValue={voteValue}&listId={listId}&voteOwnerId={voteOwnerId}");

            if (voteRequest.IsSuccess)
            {
                var jsonData = voteRequest.Data.ToString();
                var data = JsonConvert.DeserializeObject<VoteMovieList>(jsonData);
                return new CustomJsonModel { IsValid = true, Data = data, SuccessMessage = voteRequest.Message };
            }
            else
                return new CustomJsonModel { IsValid = false, ErrorMessage = voteRequest.Message };

        }

        public async Task<double> GetMovieListRank( int listId)
        {
            var listMovieRankRequest = await _apiService.GetAsync($"{baseUrl}/api/MovieList/get-listvote?listId={listId}");
            if (listMovieRankRequest.IsSuccess)
            {
                if (listMovieRankRequest.Data != null)
                {
                    var jsonData2 = listMovieRankRequest.Data.ToString();
                    return JsonConvert.DeserializeObject<double>(jsonData2); 
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

    }
}
