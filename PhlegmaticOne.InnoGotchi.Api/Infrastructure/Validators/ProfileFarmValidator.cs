using FluentValidation;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Validators;

public class ProfileFarmValidator : AbstractValidator<IdentityFarmModel>
{
    public ProfileFarmValidator(IDataService dataService)
    {
        var farmRepository = dataService.GetDataRepository<Farm>();

        RuleFor(x => x.Name)
            .MustAsync((x, _) => farmRepository.AllAsync(f => f.Name != x))
            .WithMessage(x => $"Farm name {x} already reserved");

        RuleFor(x => x.ProfileId)
            .MustAsync((x, _) => farmRepository.AllAsync(f => f.OwnerId != x))
            .WithMessage(x => $"Profile {x} already has a farm");
    }
}