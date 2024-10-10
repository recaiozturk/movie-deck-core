using FluentValidation;
using MovieBringer.Core.Models.ViewModel.Account;

namespace MovieBringer.Service.Validations
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {

            RuleFor(x => x.UserName).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("Username cannot be EmptyEE");

            RuleFor(x => x.Email).NotNull().WithMessage("{PropertyName} is required").NotEmpty().EmailAddress();

            RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Password is required")
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(16).WithMessage("Password cannot exceed 16 characters.")
                .Must(password => password.Any(char.IsUpper)).WithMessage("Password must contain at least one uppercase letter")
                .Must(password => password.Any(char.IsLower)).WithMessage("Password must contain at least one lowercase letter")
                .Must(password => password.Any(c => !char.IsLetterOrDigit(c))).WithMessage("Password must contain at least one special character");

            RuleFor(r => r.ConfirmPassword)
                .Equal(r => r.Password).WithMessage("Confirm Password must match Password");

        }
    }
}
