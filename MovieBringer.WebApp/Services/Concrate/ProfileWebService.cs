using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Models.ViewModel.Profile;
using MovieBringer.Core.Services;
using MovieBringer.WebApp.Models;
using MovieBringer.WebApp.Services.Abstract;
using MovieBringer.WebApp.Util.Abstract;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace MovieBringer.WebApp.Services.Concrate
{
    public class ProfileWebService : IProfileWebService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMovieListDTOService _movieListService;
        private readonly IMovieListService _movieListWebService;
        private readonly IMapper _mapper;
        private readonly IMethods _methods;
        private readonly IApiService _apiService;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string baseUrl = "";

        public ProfileWebService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMovieListDTOService movieListService, IMovieListService movieListWebService, IMapper mapper, IMethods methods, IApiService apiService, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _movieListService = movieListService;
            _movieListWebService = movieListWebService;
            _mapper = mapper;
            _methods = methods;
            _apiService = apiService;
            _config = config;
            baseUrl = _config.GetSection("AppSettings:WebUrl").Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProfileViewModel> GetProfileModel(string userIdInCome)
        {
            string userId = "";
            if(userIdInCome == null) 
                userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            else
                userId=userIdInCome;

            var user = await _userManager.FindByIdAsync(userId);

            var profileModel = _mapper.Map<ProfileViewModel>(user);
            var getListsRequest = await _apiService.GetAsync($"{baseUrl}/api/MovieList/get-lists?userid={userId}");

            if (getListsRequest.IsSuccess)
            {
                var jsonData = getListsRequest.Data.ToString();
                var movieLists = JsonConvert.DeserializeObject<List<MovieList>>(jsonData);

                foreach (var list in movieLists)
                {
                    list.ListRank= await _movieListWebService.GetMovieListRank(list.Id);
                }

                profileModel.MovieLists = movieLists;
            }
            else
                profileModel.MovieLists = null;

            return profileModel;
        }

        public async Task<CustomJsonModel> UpdateProfile(ProfileViewModel model)
        {
            var updateProfileResult = await _apiService.PutAsync($"{baseUrl}/api/User", model);

            if (updateProfileResult != null)
            {
                if (updateProfileResult.IsSuccess)
                {
                    //degisiklikten sonda oturumu kapat-ac
                    //mobilde ve ya client app de gerekirse apiye eklenir
                    await _signInManager.SignOutAsync();

                    var user = await _userManager.FindByIdAsync(model.Id);

                    //tekrar gir
                    if (user != null)
                    {
                        //mobilde ve ya client app de gerekirse apiye eklenir
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }

                    return new CustomJsonModel { IsValid = true, SuccessMessage = updateProfileResult.Message };
                }
                else
                {
                    return new CustomJsonModel { IsValid = false, ErrorMessages = updateProfileResult.Errors };
                }
            }

            return new CustomJsonModel { IsValid = false, ErrorMessages = new List<string> { "Error Happened" } };
        }

        public async Task<CustomJsonModel> ChangePasswordProfile(ChangePasswordViewModel model)
        {
            var changePasswordResult = await _apiService.PostAsync($"{baseUrl}/api/User/change-password", model);

            if (changePasswordResult != null)
            {
                if (changePasswordResult.IsSuccess)
                {
                    return new CustomJsonModel { IsValid = true, SuccessMessage = changePasswordResult.Message };
                }
                else
                {
                    return new CustomJsonModel { IsValid = false, ErrorMessages = changePasswordResult.Errors };
                }
            }

            return new CustomJsonModel { IsValid = false, ErrorMessages = new List<string> { "Error Happened" } };
        }
    }
}
