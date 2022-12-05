using FluentValidation;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators.Base;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class ProfileFarmModelValidator : AbstractValidator<ProfileFarmModel>
{
    public ProfileFarmModelValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.ProfileId).ProfileMustHaveFarm(unitOfWork);
    }
}