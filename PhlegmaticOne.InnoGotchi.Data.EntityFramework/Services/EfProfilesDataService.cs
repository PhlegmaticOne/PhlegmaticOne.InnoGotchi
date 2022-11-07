using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Services;

public class EfProfilesDataService : IUserProfilesDataService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IUsersDataService _usersDataService;

    public EfProfilesDataService(ApplicationDbContext applicationDbContext, IUsersDataService usersDataService)
    {
        _applicationDbContext = applicationDbContext;
        _usersDataService = usersDataService;
    }

    public async Task<UserProfile> CreateProfileAsync(UserProfile userProfile)
    {
        var user = await CreateUserFromProfile(userProfile);
        var createdUserProfile = await CreateProfile(userProfile);

        createdUserProfile.User = user;
        return createdUserProfile;
    }

    public async Task<UserProfile> GetProfileForUserAsync(User user)
    {
        var set = Set();
        return await set
            .Include(x => x.User)
            .FirstAsync(x => x.User.Password == user.Password && x.User.Email == user.Email);
    }

    private async Task<UserProfile> CreateProfile(UserProfile userProfile)
    {
        var profilesSet = Set();
        var entity = await profilesSet.AddAsync(userProfile);
        await _applicationDbContext.SaveChangesAsync();
        return entity.Entity;
    }

    private async Task<User> CreateUserFromProfile(UserProfile userProfile) => 
        await _usersDataService.CreateUserAsync(userProfile.User);

    private DbSet<UserProfile> Set() => _applicationDbContext.Set<UserProfile>();
}