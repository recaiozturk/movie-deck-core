using MovieBringer.Core.Models.ViewModel.Profile;
using MovieBringer.WebApp.Models;

namespace MovieBringer.WebApp.Services.Abstract
{
    public interface IProfileWebService
    {
        Task<ProfileViewModel> GetProfileModel(string userId);
        Task<CustomJsonModel> UpdateProfile(ProfileViewModel model);
        Task<CustomJsonModel> ChangePasswordProfile(ChangePasswordViewModel model);
    }
}
