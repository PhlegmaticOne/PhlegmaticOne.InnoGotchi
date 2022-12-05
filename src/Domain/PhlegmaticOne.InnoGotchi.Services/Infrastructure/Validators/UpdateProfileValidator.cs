using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Commands.Profiles;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators.Base;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.PasswordHasher;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileValidator(IUnitOfWork dataService, IPasswordHasher passwordHasher)
    {
        var profilesRepository = dataService.GetRepository<UserProfile>();

        RuleFor(x => x.UpdateProfileDto.FirstName)
            .MinimumLength(3)
            .When(x => string.IsNullOrEmpty(x.UpdateProfileDto.FirstName) == false)
            .WithMessage(AppErrorMessages.NameIsTooShortMessage)
            .MaximumLength(50)
            .When(x => string.IsNullOrEmpty(x.UpdateProfileDto.FirstName) == false)
            .WithMessage(AppErrorMessages.NameIsTooLongMessage);

        RuleFor(x => x.UpdateProfileDto.LastName)
            .MinimumLength(3)
            .When(x => string.IsNullOrEmpty(x.UpdateProfileDto.LastName) == false)
            .WithMessage(AppErrorMessages.NameIsTooShortMessage)
            .MaximumLength(50)
            .When(x => string.IsNullOrEmpty(x.UpdateProfileDto.LastName) == false)
            .WithMessage(AppErrorMessages.NameIsTooLongMessage);

        RuleFor(x => x.UpdateProfileDto.NewPassword)
            .Password(10)
            .When(x => string.IsNullOrEmpty(x.UpdateProfileDto.OldPassword) == false);

        RuleFor(x => x.UpdateProfileDto.OldPassword)
            .MustAsync(async (profile, oldPassword, ct) =>
            {
                var password = passwordHasher.Hash(oldPassword);
                return await profilesRepository
                    .ExistsAsync(p => p.Id == profile.ProfileId && p.User.Password == password, ct);
            })
            .When(x => string.IsNullOrEmpty(x.UpdateProfileDto.OldPassword) == false)
            .WithMessage(AppErrorMessages.OldPasswordIsIncorrectMessage);
    }
}