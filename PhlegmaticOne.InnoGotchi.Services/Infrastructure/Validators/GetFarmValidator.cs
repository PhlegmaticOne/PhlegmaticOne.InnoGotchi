using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class GetFarmValidator : AbstractValidator<GetFarmModel>
{
    public GetFarmValidator(IUnitOfWork unitOfWork)
    {
        var farmRepository = unitOfWork.GetRepository<Farm>();
        RuleFor(x => x.ProfileId)
            .MustAsync((id, _) => farmRepository.ExistsAsync(x => x.OwnerId == id))
            .WithMessage(x => $"Account {x} doesn't have a farm");
    }
}