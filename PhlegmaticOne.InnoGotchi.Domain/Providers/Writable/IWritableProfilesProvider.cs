using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableProfilesProvider
{
    Task<UserProfile> CreateAsync(RegisterProfileDto registerProfileDto);
    Task<UserProfile> UpdateAsync(IdentityModel<UpdateProfileDto> updateProfileDto);
}