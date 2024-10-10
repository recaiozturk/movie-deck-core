
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Models.ViewModel.Account;
using MovieBringer.WebApp.Services.Abstract;
using System.Web;


namespace MovieBringer.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiService _apiService;
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly string baseUrl = "";

        public AccountController(IApiService apiService, IConfiguration config , UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _apiService = apiService;
            _config = config;
            baseUrl = _config.GetSection("AppSettings:WebUrl").Value;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {           

            if (ModelState.IsValid)
            {
                var registerResponse = await _apiService.PostAsync($"{baseUrl}/api/Auth/register", model);

                if (registerResponse.IsSuccess)
                {
                    TempData["message"] = "Registration is successful. Click on the confirmation link sent to your e-mail.";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.Clear();
                    foreach (var err in registerResponse.Errors)
                    {
                        ModelState.AddModelError("CustomErrorKey", err);
                    }
                }

            }
            return View(model);
        }

        public async Task<IActionResult> Login(string ReturnUrl = "noReturnUrl")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var loginRequest = await _apiService.PostAsync($"{baseUrl}/api/Auth/login", model);

                if (loginRequest.IsSuccess)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddHours(1)
                    };

                    var token = loginRequest.Data.ToString();

                    Response.Cookies.Append("AccessToken", token, cookieOptions);

                    await _signInManager.SignOutAsync();
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null);
                    }

                    TempData["message"] = loginRequest.Message;
                    TempData["messageType"] = "success";

                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && model.ReturnUrl != "noReturnUrl")
                        return Redirect(model.ReturnUrl);
                    else
                        return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.Clear();
                    foreach (var err in loginRequest.Errors)
                    {
                        ModelState.AddModelError("CustomErrorKey", err);
                    }
                }
            }

            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            var logOutrequest = await _apiService.GetAsync($"{_config.GetSection("AppSettings:WebUrl").Value}/api/Auth/log-out");

            if (logOutrequest.IsSuccess)
            {
                Response.Cookies.Delete("AccessToken");
                await _signInManager.SignOutAsync();

                TempData["message"] = logOutrequest.Message;
                TempData["messageType"] = "success";

                return RedirectToAction("Login");
            }

            TempData["message"] = logOutrequest.Message;
            TempData["messageType"] = "danger";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData["message"] = "Invalid token information";
                TempData["messageType"] = "danger";
                return RedirectToAction("Error", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                token = HttpUtility.UrlDecode(token);
                token = token.Replace(" ", "+");
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    TempData["message"] = "Your Account Has Been Confirmed";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                TempData["message"] = "User not found";
                TempData["messageType"] = "danger";
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Error", "Home");
        }


        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var url = $"{baseUrl}/api/Auth/generate-forget-password";
                var forgetPasswordRequest = await _apiService.PostAsync(url, model);

                if (forgetPasswordRequest.IsSuccess)
                {
                    TempData["message"] = "You can reset your password with the link sent to your email.";
                    TempData["messageType"] = "success";
                    return View();
                }
                else
                {
                    TempData["message"] = "Unextepted Error";
                    TempData["messageType"] = "danger";
                    return RedirectToAction("Login");
                }
            }

            return View(model);
        }

        public ActionResult ResetPassword(string Id, string token)
        {
            if (Id == null || token == null)
                return RedirectToAction("Login");

            var model = new ResetPasswordViewModel { Token = token, UserId = Id };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var url = $"{baseUrl}/api/Auth/verify-reset-password";
                var forgetPasswordRequest = await _apiService.PostAsync(url, model);

                if (forgetPasswordRequest.IsSuccess)
                {
                    TempData["message"] = "Your password has been changed.";
                    TempData["messageType"] = "success";
                    return RedirectToAction("Login");
                }
            }
            return View(model);

        }

    }
}
