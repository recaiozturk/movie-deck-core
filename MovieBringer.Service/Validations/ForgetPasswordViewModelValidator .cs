using FluentValidation;
using MovieBringer.Core.Models.ViewModel.Account;

namespace MovieBringer.Service.Validations
{
    public class ForgetPasswordViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public ForgetPasswordViewModelValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("{PropertyName} is required").NotEmpty().EmailAddress();
        }
    }
}
