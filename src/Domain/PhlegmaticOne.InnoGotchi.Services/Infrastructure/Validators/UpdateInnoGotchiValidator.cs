using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Services.Commands.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class UpdateInnoGotchiValidator : AbstractValidator<UpdateInnoGotchiCommand>
{
    public UpdateInnoGotchiValidator(IInnoGotchiOwnChecker innoGotchiOwnChecker, IUnitOfWork unitOfWork)
    {
        var petsRepository = unitOfWork.GetRepository<InnoGotchiModel>();

        RuleFor(x => x)
            .MustAsync(async (model, ct) =>
                await innoGotchiOwnChecker.IsBelongAsync(model.ProfileId, model.UpdateInnoGotchiDto.PetId, ct))
            .WithMessage(AppErrorMessages.PetDoesNotBelongToProfileMessage);

        RuleFor(x => x.UpdateInnoGotchiDto.PetId)
            .MustAsync(async (id, ct) =>
                await petsRepository.ExistsAsync(x => x.Id == id && x.IsDead == false, ct))
            .WithMessage(AppErrorMessages.CannotUpdateDeadPetMessage);
    }
}