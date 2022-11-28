using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class GetCollaboratedFarmValidator : AbstractValidator<IdentityModel<GetFarmModel>>
{
    public GetCollaboratedFarmValidator(IUnitOfWork unitOfWork)
    {
        var collaborationsRepository = unitOfWork.GetRepository<Collaboration>();

        RuleFor(x => x)
            .MustAsync((model, _) =>
                collaborationsRepository.ExistsAsync(x =>
                    x.Farm.OwnerId == model.Entity.ProfileId && x.UserProfileId == model.ProfileId))
            .WithMessage(x => "You haven't such collaboration");
    }
}