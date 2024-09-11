
using System.ComponentModel.DataAnnotations;


namespace MovieBringer.Core.Models.ViewModel.Profile
{
    public class ChangePasswordViewModel
    {
        public string? Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string NewPasswordConfirm { get; set; } = null!;
    }
}
