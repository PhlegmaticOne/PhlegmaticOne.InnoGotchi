using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Queries.Farms;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class GetFarmValidator : AbstractValidator<GetFarmByIdQuery>
{
    public GetFarmValidator(IUnitOfWork unitOfWork)
    {
        var collaborationsRepository = unitOfWork.GetRepository<Collaboration>();

        RuleFor(x => x)
            .MustAsync((model, ct) =>
                collaborationsRepository.ExistsAsync(x => x.UserProfileId == model.ProfileId &&
                                                          x.FarmId == model.FarmId, ct));
    }
}