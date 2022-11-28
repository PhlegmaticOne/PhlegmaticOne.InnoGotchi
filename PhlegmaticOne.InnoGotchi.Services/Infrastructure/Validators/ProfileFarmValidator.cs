using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class ProfileFarmValidator : AbstractValidator<IdentityModel<CreateFarmDto>>
{
    public ProfileFarmValidator(IUnitOfWork dataService)
    {
        var farmRepository = dataService.GetRepository<Farm>();
        
        RuleFor(x => x.Entity.Name)
            .MustAsync((x, _) => farmRepository.AllAsync(f => f.Name != x))
            .WithMessage(x => $"Farm name {x.Entity.Name} already reserved");

        RuleFor(x => x.ProfileId)
            .MustAsync((x, _) => farmRepository.AllAsync(f => f.Owner.Id != x))
            .WithMessage("You already have a farm");
    }
}