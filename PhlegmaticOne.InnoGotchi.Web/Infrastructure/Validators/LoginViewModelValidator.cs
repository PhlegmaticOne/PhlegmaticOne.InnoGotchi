using FluentValidation;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Validators;

public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginViewModelValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Write correct email address");

        RuleFor(x => x.Password)
            .Password(10);
    }
}