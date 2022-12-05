using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Commands.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators.Base;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class CreateInnoGotchiValidator : AbstractValidator<CreateInnoGotchiCommand>
{
    public CreateInnoGotchiValidator(IUnitOfWork unitOfWork)
    {
        var petsRepository = unitOfWork.GetRepository<InnoGotchiModel>();
        var componentsRepository = unitOfWork.GetRepository<InnoGotchiComponent>();

        RuleFor(x => x.ProfileId).ProfileMustHaveFarm(unitOfWork);

        RuleFor(x => x.CreateInnoGotchiDto.Name)
            .MustAsync(async (name, ct) => await petsRepository.AllAsync(x => x.Name != name, ct))
            .WithMessage(AppErrorMessages.InnoGotchiNameReservedMessage)
            .MinimumLength(3)
            .WithMessage(AppErrorMessages.NameIsTooShortMessage)
            .MaximumLength(40)
            .WithMessage(AppErrorMessages.NameIsTooLongMessage);

        RuleFor(x => x.CreateInnoGotchiDto.Components)
            .Must(components => components.Any(component => component.Name == "Bodies"))
            .WithMessage(AppErrorMessages.InnoGotchiMustHaveBodyMessage)
            .MustAsync(async (c, ct) =>
            {
                var categories = c.Select(x => x.Name).ToList();
                if (categories.Any() == false)
                {
                    return false;
                }
                return await componentsRepository.ExistsAsync(x => categories.Contains(x.Name), ct);
            })
            .WithMessage(AppErrorMessages.UnknownComponentCategoryNameMessage)
            .MustAsync(async (c, ct) =>
            {
                var urls = c.Select(x => x.ImageUrl).ToList();
                if (urls.Any() == false)
                {
                    return false;
                }
                return await componentsRepository.ExistsAsync(x => urls.Contains(x.ImageUrl), ct);
            })
            .WithMessage(AppErrorMessages.UnknownComponentImageUrlMessage);
    }
}