
namespace MovieBringer.Core.Models.ViewModel.Account
{
    public class LoginViewModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; } = true;
        public string? ReturnUrl { get; set; }
    }
}
