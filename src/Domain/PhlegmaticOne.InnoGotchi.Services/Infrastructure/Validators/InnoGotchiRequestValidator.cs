using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class InnoGotchiRequestValidator : AbstractValidator<IdentityModel<InnoGotchiRequestDto>>
{
    public InnoGotchiRequestValidator(IInnoGotchiOwnChecker innoGotchiOwnChecker)
    {
        RuleFor(x => x)
            .MustAsync((model, ct) =>
                innoGotchiOwnChecker.IsBelongAsync(model.ProfileId, model.Entity.PetId, ct))
            .WithMessage("You can't make any actions with this InnoGotchi");
    }
}