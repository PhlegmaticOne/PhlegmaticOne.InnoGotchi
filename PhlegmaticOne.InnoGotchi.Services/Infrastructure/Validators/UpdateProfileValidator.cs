using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.PasswordHasher.Base;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileDto>
{
    public UpdateProfileValidator(IUnitOfWork dataService, IPasswordHasher passwordHasher)
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