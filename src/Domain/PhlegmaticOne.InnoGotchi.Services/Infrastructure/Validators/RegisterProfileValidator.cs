using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Commands.Profiles;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class RegisterProfileValidator : AbstractValidator<RegisterProfileCommand>
{
    public RegisterProfileValidator(IUnitOfWork dataService)
    {
        var userProfilesRepository = dataService.GetRepository<UserProfile>();

        RuleFor(x => x.RegisterProfileModel.Email)
            .MustAsync((email, _) => userProfilesRepository.AllAsync(profile => profile.User.Email != email))
            .WithMessage("Unable to create user profile. User with email exists");
    }
}