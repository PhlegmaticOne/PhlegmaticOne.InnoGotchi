using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class ExistsProfileFarmValidator : AbstractValidator<ExistsProfileFarmModel>
{
    public ExistsProfileFarmValidator(IUnitOfWork unitOfWork)
    {
        var farmsRepository = unitOfWork.GetRepository<Farm>();

        RuleFor(x => x.ProfileId)
            .MustAsync((id, ct) => farmsRepository.ExistsAsync(x => x.OwnerId == id, ct))
            .WithMessage(AppErrorMessages.FarmDoesNotExistMessage);
    }
}