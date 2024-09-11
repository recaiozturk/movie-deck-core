using Microsoft.AspNetCore.Mvc;
using MovieBringer.Core.Models.ViewModel.Profile;
using MovieBringer.WebApp.Models;
using MovieBringer.WebApp.Services.Abstract;
using MovieBringer.WebApp.Util.Abstract;

namespace MovieBringer.WebApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileWebService _profileService;
        private readonly IMethods _methods;

        public ProfileController(IProfileWebService profileService, IMethods methods)
        {
            _profileService = profileService;
            _methods = methods;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var profileModel = await _profileService.GetProfileModel(null);
                return View(profileModel);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UserProfile(string userId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var profileModel = await _profileService.GetProfileModel(userId);
                return View(profileModel);
            }
            else
            {
                TempData["message"] = "Please log in to access users' profiles";
                TempData["messageType"] = "danger";
                return RedirectToAction("Index", "Home");
            }
                
        }

        [HttpPost]
        public async Task<JsonResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new CustomJsonModel{IsValid = false,ErrorMessages = _methods.ModelErrors(ModelState)});

            return Json(await _profileService.UpdateProfile(model));
        }

        [HttpPost]
        public async Task<JsonResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new CustomJsonModel { IsValid = false, ErrorMessages = _methods.ModelErrors(ModelState) });

            return Json(await _profileService.ChangePasswordProfile(model));
        }
    }
}
