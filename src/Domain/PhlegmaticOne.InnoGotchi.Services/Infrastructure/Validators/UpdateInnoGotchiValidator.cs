using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Services.Commands.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Helpers;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class UpdateInnoGotchiValidator : AbstractValidator<UpdateInnoGotchiCommand>
{
    public UpdateInnoGotchiValidator(IInnoGotchiOwnChecker innoGotchiOwnChecker)
    {
        RuleFor(x => x)
            .MustAsync((model, ct) =>
                innoGotchiOwnChecker.IsBelongAsync(model.ProfileId, model.UpdateInnoGotchiDto.PetId, ct))
            .WithMessage("You can't make any actions with this InnoGotchi");

        RuleFor(x => x.UpdateInnoGotchiDto.InnoGotchiOperationType)
            .Must(type => (int)type != EnumHelper.MaxValue(type))
            .WithMessage("You can't update dead InnoGotchi");
    }
}