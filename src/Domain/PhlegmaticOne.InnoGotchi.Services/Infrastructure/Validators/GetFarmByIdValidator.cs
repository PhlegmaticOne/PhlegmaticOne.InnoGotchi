using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Queries.Farms;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class GetFarmByIdValidator : AbstractValidator<GetFarmByIdQuery>
{
    public GetFarmByIdValidator(IUnitOfWork unitOfWork)
    {
        var collaborationsRepository = unitOfWork.GetRepository<Collaboration>();

        RuleFor(x => x)
            .MustAsync(async (model, ct) => 
                await collaborationsRepository
                    .ExistsAsync(x => x.UserProfileId == model.ProfileId && x.FarmId == model.FarmId, ct))
            .WithMessage(AppErrorMessages.CannotGetFarmBecauseOfCollaboration);
    }
}