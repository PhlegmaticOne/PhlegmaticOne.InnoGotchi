using FluentValidation;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Validators;

public class CreateInnoGotchiViewModelValidator : AbstractValidator<CreateInnoGotchiViewModel>
{
    public CreateInnoGotchiViewModelValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .WithMessage("Name is too short")
            .MaximumLength(40)
            .WithMessage("Name is too long");

        RuleFor(x => x.Components)
            .Must(components => components.Any(component => component.Name == "Bodies"))
            .WithMessage("Your InnoGotchi must have a body");
    }
}