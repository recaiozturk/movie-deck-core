
namespace MovieBringer.Core.Models.ViewModel.Account
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
