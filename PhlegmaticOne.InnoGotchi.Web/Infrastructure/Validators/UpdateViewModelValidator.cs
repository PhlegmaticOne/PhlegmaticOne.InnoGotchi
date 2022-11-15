using FluentValidation;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Validators;

public class UpdateViewModelValidator : AbstractValidator<UpdateAccountViewModel>
{
    public UpdateViewModelValidator()
    {
        RuleFor(x => x.FirstName)
            .MinimumLength(3)
            .MaximumLength(50)
            .When(x => string.IsNullOrEmpty(x.FirstName) == false)
            .WithMessage("Specify firstname with length of 3 to 50");

        RuleFor(x => x.LastName)
            .MinimumLength(3)
            .MaximumLength(50)
            .When(x => string.IsNullOrEmpty(x.LastName) == false)
            .WithMessage("Specify lastname with length of 3 to 50");

        RuleFor(x => x.NewPassword!)
            .Password(10)
            .When(x => string.IsNullOrEmpty(x.OldPassword) == false);

        RuleFor(x => x.NewPasswordConfirm)
            .Equal(x => x.NewPassword)
            .When(x => string.IsNullOrEmpty(x.OldPassword) == false)
            .WithMessage("Passwords aren't equal");
    }
}