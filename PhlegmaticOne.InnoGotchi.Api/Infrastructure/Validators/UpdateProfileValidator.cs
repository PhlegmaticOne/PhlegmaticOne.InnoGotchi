using FluentValidation;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.PasswordHasher.Base;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Validators;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileDto>
{
    public UpdateProfileValidator(IDataService dataService, IPasswordHasher passwordHasher)
    {
        var profilesRepository = dataService.GetDataRepository<UserProfile>();
        RuleFor(x => x.OldPassword)
            .MustAsync((profile, oldPassword, _) =>
            {
                var password = passwordHasher.Hash(oldPassword);
                return profilesRepository.ExistsAsync(p => p.Id == profile.Id && p.User.Password == password);
            })
            .When(x => string.IsNullOrEmpty(x.OldPassword) == false)
            .WithMessage("Old password is incorrect");
    }
}