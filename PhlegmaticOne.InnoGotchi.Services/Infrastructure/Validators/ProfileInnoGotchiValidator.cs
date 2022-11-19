using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class ProfileInnoGotchiValidator : AbstractValidator<IdentityModel<CreateInnoGotchiDto>>
{
    public ProfileInnoGotchiValidator(IUnitOfWork dataService)
    {
        var farmRepository = dataService.GetDataRepository<Farm>();
        var petsRepository = dataService.GetDataRepository<InnoGotchiModel>();

        RuleFor(x => x.ProfileId)
            .MustAsync((profileId, _) => farmRepository.ExistsAsync(f => f.OwnerId == profileId))
            .WithMessage("You need to create farm for storing InnoGotchies");

        RuleFor(x => x.Entity.Name)
            .MustAsync((model, name, _) =>
                petsRepository.AllAsync(x => x.Farm.OwnerId != model.ProfileId && x.Name != name))
            .WithMessage(x => $"You have InnoGotchi with name {x.Entity.Name}");
    }
}