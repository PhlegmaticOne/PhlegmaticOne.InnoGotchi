using FluentValidation;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Validators;

public class UserProfileValidator : AbstractValidator<RegisterProfileDto>
{
    public UserProfileValidator(IDataService dataService)
    {
        var userProfilesRepository = dataService.GetDataRepository<UserProfile>();

        RuleFor(x => x.Email)
            .MustAsync((email, _) => userProfilesRepository.AllAsync(profile => profile.User.Email != email))
            .WithMessage(x => $"Unable to create user profile. User with email exists: {x.Email}");
    }
}