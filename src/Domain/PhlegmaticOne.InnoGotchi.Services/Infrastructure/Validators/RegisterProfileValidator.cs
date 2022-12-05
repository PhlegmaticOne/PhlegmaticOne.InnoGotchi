using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Commands.Profiles;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators.Base;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class RegisterProfileValidator : AbstractValidator<RegisterProfileCommand>
{
    public RegisterProfileValidator(IUnitOfWork dataService)
    {
        var userProfilesRepository = dataService.GetRepository<UserProfile>();

        RuleFor(x => x.RegisterProfileModel.Email)
            .EmailAddress()
            .WithMessage(AppErrorMessages.EmailIncorrectMessage)
            .MustAsync(async (email, ct) => await userProfilesRepository.AllAsync(profile => profile.User.Email != email))
            .WithMessage(AppErrorMessages.EmailExistsMessage);

        RuleFor(x => x.RegisterProfileModel.Password)
            .Password(10)
            .WithMessage(AppErrorMessages.PasswordIncorrectMessage);

        RuleFor(x => x.RegisterProfileModel.FirstName)
            .MinimumLength(3)
            .WithMessage(AppErrorMessages.NameIsTooShortMessage)
            .MaximumLength(50)
            .WithMessage(AppErrorMessages.NameIsTooLongMessage);

        RuleFor(x => x.RegisterProfileModel.LastName)
            .MinimumLength(3)
            .WithMessage(AppErrorMessages.NameIsTooShortMessage)
            .MaximumLength(50)
            .WithMessage(AppErrorMessages.NameIsTooLongMessage);
    }
}