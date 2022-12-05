using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Commands.Farms;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class CreateFarmValidator : AbstractValidator<CreateFarmCommand>
{
    public CreateFarmValidator(IUnitOfWork dataService)
    {
        var farmRepository = dataService.GetRepository<Farm>();

        RuleFor(x => x.ProfileId)
            .MustAsync(async (x, ct) => await farmRepository.AllAsync(f => f.OwnerId != x, ct))
            .WithMessage(AppErrorMessages.AlreadyHaveFarmMessage);

        RuleFor(x => x.CreateFarmDto.Name)
            .MustAsync(async (x, ct) => await farmRepository.AllAsync(f => f.Name != x, ct))
            .WithMessage(AppErrorMessages.FarmNameReservedMessage)
            .MinimumLength(3)
            .WithMessage(AppErrorMessages.NameIsTooShortMessage)
            .MaximumLength(40)
            .WithMessage(AppErrorMessages.NameIsTooLongMessage);
    }
}