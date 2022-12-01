using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Commands.Profiles;
using PhlegmaticOne.PasswordHasher;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileValidator(IUnitOfWork dataService, IPasswordHasher passwordHasher)
    {
        var profilesRepository = dataService.GetRepository<UserProfile>();
        RuleFor(x => x.UpdateProfileDto.OldPassword)
            .MustAsync((profile, oldPassword, _) =>
            {
                var password = passwordHasher.Hash(oldPassword);
                return profilesRepository
                    .ExistsAsync(p => p.Id == profile.ProfileId && p.User.Password == password);
            })
            .When(x => string.IsNullOrEmpty(x.UpdateProfileDto.OldPassword) == false)
            .WithMessage("Old password is incorrect");
    }
}