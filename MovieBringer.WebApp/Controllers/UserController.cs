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
    }
}
