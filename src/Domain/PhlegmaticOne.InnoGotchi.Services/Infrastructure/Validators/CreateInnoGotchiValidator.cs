using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Commands.InnoGotchies;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class CreateInnoGotchiValidator : AbstractValidator<CreateInnoGotchiCommand>
{
    public CreateInnoGotchiValidator(IUnitOfWork dataService)
    {
        var farmRepository = dataService.GetRepository<Farm>();
        var petsRepository = dataService.GetRepository<InnoGotchiModel>();

        RuleFor(x => x.ProfileId)
            .MustAsync((profileId, ct) => farmRepository.ExistsAsync(f => f.Owner.Id == profileId, ct))
            .WithMessage("You need to create farm for storing InnoGotchies");

        RuleFor(x => x.CreateInnoGotchiDto.Name)
            .MustAsync((name, ct) =>
                petsRepository.AllAsync(x => x.Name != name, ct))
            .WithMessage("InnoGotchi name reserved");
    }
}