using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class UserProfileValidator : AbstractValidator<RegisterProfileDto>
{
    public UserProfileValidator(IUnitOfWork dataService)
    {
        var userProfilesRepository = dataService.GetDataRepository<UserProfile>();

        RuleFor(x => x.Email)
            .MustAsync((email, _) => userProfilesRepository.AllAsync(profile => profile.User.Email != email))
            .WithMessage(x => $"Unable to create user profile. User with email exists: {x.Email}");
    }
}