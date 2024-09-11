using MovieBringer.Core.DTOs;
using MovieBringer.Core.Models.ViewModel.Account;



namespace MovieBringer.Core.Services
{
    public interface IAuthDtoService
    {
        Task<CustomResponseDto<NoContentDto>> CreateUserAsync(RegisterViewModel registerViewModel);
        Task<CustomResponseDto<object>> Login(LoginViewModel model);
        Task<CustomResponseDto<NoContentDto>> ForgotPassword(ForgetPasswordViewModel model);
        Task<CustomResponseDto<NoContentDto>> ResetPassword(ResetPasswordViewModel model);
        Task<CustomResponseDto<NoContentDto>> LogOut();
    }
}
