using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Services;

public class EfProfilesDataService : IUserProfilesDataService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public EfProfilesDataService(ApplicationDbContext applicationDbContext) => 
        _applicationDbContext = applicationDbContext;

    public async Task<UserProfile> CreateProfileAsync(UserProfile userProfile)
    {
        var entity = await Set().AddAsync(userProfile);
        await _applicationDbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<UserProfile> GetProfileForUserAsync(User user) =>
        await Set()
            .Include(x => x.User)
            .FirstAsync(x => x.User.Password == user.Password && x.User.Email == user.Email);

    private DbSet<UserProfile> Set() => _applicationDbContext.Set<UserProfile>();
}