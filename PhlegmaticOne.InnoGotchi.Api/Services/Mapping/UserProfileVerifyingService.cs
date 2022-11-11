using FluentValidation;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Services.Mapping.Base;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.PasswordHasher.Base;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Mapping;

public class UserProfileVerifyingService : VerifyingServiceBase<RegisterProfileDto, UserProfile>
{
    private readonly IPasswordHasher _passwordHasher;

    public UserProfileVerifyingService(IValidator<RegisterProfileDto> fluentValidator, IDataService dataService, 
        IPasswordHasher passwordHasher) :
        base(fluentValidator, dataService) =>
        _passwordHasher = passwordHasher;

    public override Task<UserProfile> MapAsync(RegisterProfileDto from)
    {
        return Task.FromResult(new UserProfile
        {
            User = new User
            {
                Email = from.Email,
                Password = _passwordHasher.Hash(from.Password)
            },
            Avatar = new Avatar
            {
                AvatarData = from.AvatarData
            },
            JoinDate = DateTime.UtcNow,
            FirstName = from.FirstName,
            LastName = from.LastName
        });
    }
}