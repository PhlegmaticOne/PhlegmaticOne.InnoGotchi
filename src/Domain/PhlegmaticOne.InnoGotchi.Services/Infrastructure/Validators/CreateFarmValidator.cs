using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Commands.Farms;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class CreateFarmValidator : AbstractValidator<CreateFarmCommand>
{
    public CreateFarmValidator(IUnitOfWork dataService)
    {
        var farmRepository = dataService.GetRepository<Farm>();

        RuleFor(x => x.CreateFarmDto.Name)
            .MustAsync((x, ct) => farmRepository.AllAsync(f => f.Name != x, ct))
            .WithMessage("Farm name already reserved");

        RuleFor(x => x.ProfileId)
            .MustAsync((x, ct) => farmRepository.AllAsync(f => f.Owner.Id != x, ct))
            .WithMessage("You already have a farm");
    }
}