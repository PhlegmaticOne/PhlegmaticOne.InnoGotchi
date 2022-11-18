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
        var componentsRepository = dataService.GetDataRepository<InnoGotchiComponent>();
        var farmRepository = dataService.GetDataRepository<Farm>();

        RuleFor(x => x.ProfileId)
            .MustAsync((profileId, _) => farmRepository.ExistsAsync(f => f.OwnerId == profileId))
            .WithMessage("You need to create farm for storing InnoGotchies");

        RuleFor(x => x.Entity.Components)
            .MustAsync(async (components, _) =>
            {
                var validPaths = await componentsRepository.GetAllAsync(x => x.ImageUrl);
                return components.All(x => validPaths.Contains(x.ImageUrl));
            })
            .WithMessage("Component paths are incorrect");
    }
}