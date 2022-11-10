using FluentValidation;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Helpers;
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
            .NotEmpty()
            .WithMessage("First name cannot be empty")
            .MaximumLength(50)
            .WithMessage("Too long first name");

        RuleFor(x => x.SecondName)
            .NotEmpty()
            .WithMessage("Second name cannot be empty")
            .MaximumLength(50)
            .WithMessage("Too long second name");
    }
}