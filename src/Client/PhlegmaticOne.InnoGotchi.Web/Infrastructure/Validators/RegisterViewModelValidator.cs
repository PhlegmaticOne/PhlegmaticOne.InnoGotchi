using FluentValidation;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Validators;

public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterViewModelValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Write correct email");

        RuleFor(x => x.Password)
            .Password(10);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords don't match");

        RuleFor(x => x.FirstName)
            .MinimumLength(3)
            .MaximumLength(50)
            .When(x => string.IsNullOrEmpty(x.FirstName))
            .WithMessage("Specify firstname with length of 3 to 50");

        RuleFor(x => x.LastName)
            .MinimumLength(3)
            .MaximumLength(50)
            .When(x => string.IsNullOrEmpty(x.FirstName))
            .WithMessage("Specify firstname with length of 3 to 50");
    }
}