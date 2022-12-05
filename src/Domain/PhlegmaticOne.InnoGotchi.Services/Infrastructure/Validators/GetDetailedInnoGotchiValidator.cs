using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Services.Queries.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class GetDetailedInnoGotchiValidator : AbstractValidator<GetDetailedInnoGotchiQuery>
{
    public GetDetailedInnoGotchiValidator(IInnoGotchiOwnChecker innoGotchiOwnChecker)
    {
        RuleFor(x => x)
            .MustAsync(async (model, ct) => await 
                innoGotchiOwnChecker.IsBelongAsync(model.ProfileId, model.PetId, ct))
            .WithMessage(AppErrorMessages.PetDoesNotBelongToProfileMessage);
    }
}