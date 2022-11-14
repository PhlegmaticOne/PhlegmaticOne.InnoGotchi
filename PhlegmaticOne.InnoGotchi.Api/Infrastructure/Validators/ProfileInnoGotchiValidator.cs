using FluentValidation;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Validators;

public class ProfileInnoGotchiValidator : AbstractValidator<IdentityInnoGotchiModel>
{
    public ProfileInnoGotchiValidator(IDataService dataService)
    {
        var componentsRepository = dataService.GetDataRepository<InnoGotchiComponent>();
        var farmRepository = dataService.GetDataRepository<Farm>();

        RuleFor(x => x.ProfileId)
            .MustAsync((profileId, _) => farmRepository.ExistsAsync(f => f.OwnerId == profileId))
            .WithMessage("You need to create farm for storing InnoGotchies");

        RuleFor(x => x.Components)
            .MustAsync(async (components, _) =>
            {
                var validPaths = await componentsRepository.GetAllAsync(x => x.ImageUrl);
                return components.All(x => validPaths.Contains(x.ImageUrl));
            })
            .WithMessage("Component paths are incorrect");
    }
}