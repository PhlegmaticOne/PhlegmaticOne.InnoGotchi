using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.PasswordHasher.Base;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class UpdateProfileValidator : AbstractValidator<IdentityModel<UpdateProfileDto>>
{
    public UpdateProfileValidator(IUnitOfWork dataService, IPasswordHasher passwordHasher)
    {
        var profilesRepository = dataService.GetRepository<UserProfile>();
        RuleFor(x => x.Entity.OldPassword)
            .MustAsync((profile, oldPassword, _) =>
            {
                var password = passwordHasher.Hash(oldPassword);
                return profilesRepository.ExistsAsync(p => p.Id == profile.ProfileId && p.User.Password == password);
            })
            .When(x => string.IsNullOrEmpty(x.Entity.OldPassword) == false)
            .WithMessage("Old password is incorrect");
    }
}