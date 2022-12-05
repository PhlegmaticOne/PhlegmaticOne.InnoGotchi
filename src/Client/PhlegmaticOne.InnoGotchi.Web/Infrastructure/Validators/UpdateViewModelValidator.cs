using FluentValidation;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Validators;

public class UpdateViewModelValidator : AbstractValidator<UpdateAccountViewModel>
{
    public UpdateViewModelValidator()
    {
        RuleFor(x => x.NewPasswordConfirm)
            .Equal(x => x.NewPassword)
            .When(x => string.IsNullOrEmpty(x.OldPassword) == false)
            .WithMessage("Passwords aren't equal");
    }
}