using Microsoft.AspNetCore.Mvc;
using MovieBringer.WebApp.Services.Abstract;

namespace MovieBringer.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;


        public UserController(IApiService apiService)
        {
            _apiService = apiService;
        }
        //test amaclı daha sonra admin için uyarlancak
        //public async Task<IActionResult> Index()
        //{
        //    //test get users
        //    //var url = $"{"https://localhost:7153"}/api/User";
        //    //var usersRequest = await _apiService.GetAsync(url);
        //    //var users = usersRequest.Data;
        //    //return View();
        //}
    }
}
