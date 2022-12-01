using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class InnoGotchiRequestValidator : AbstractValidator<IdentityModel<InnoGotchiRequestDto>>
{
    public InnoGotchiRequestValidator(IUnitOfWork unitOfWork)
    {
        var innoGotchiesRepository = unitOfWork.GetRepository<InnoGotchiModel>();
        var collaborationsRepository = unitOfWork.GetRepository<Collaboration>();

        RuleFor(x => x)
            .MustAsync(async (model, _) =>
            {
                var isPetBelongToProfile = await innoGotchiesRepository.ExistsAsync(x =>
                    x.Farm.OwnerId == model.ProfileId && x.Id == model.Entity.PetId);

                if (isPetBelongToProfile)
                {
                    return true;
                }

                var isPetFromCollaboratedFarm = await collaborationsRepository.ExistsAsync(
                    x => x.UserProfileId == model.ProfileId &&
                         x.Farm.InnoGotchies.Select(i => i.Id).Contains(model.Entity.PetId));

                return isPetFromCollaboratedFarm;
            })
            .WithMessage("You can't make any actions with this InnoGotchi");
    }
}