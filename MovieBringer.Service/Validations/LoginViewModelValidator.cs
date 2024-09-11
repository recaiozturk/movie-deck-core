using FluentValidation;
using MovieBringer.Core.Models.ViewModel.Account;
using System.Text.RegularExpressions;


namespace MovieBringer.Service.Validations
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("{PropertyName} is required").NotEmpty().EmailAddress();

            RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Password is required")
                .NotEmpty().WithMessage("Password cannot be empty.");

        }
    }
}
