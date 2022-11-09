using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.Core.Services;

public interface IUserProfilesDataService
{
    Task<UserProfile> CreateProfileAsync(UserProfile createUserDto);
    Task<UserProfile?> GetProfileForUserAsync(User user);
    Task<UserProfile?> GetProfileByEmailAsync(string email);
}