using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Services;

public class EfUsersDataService : IUsersDataService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public EfUsersDataService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public async Task<bool> IsExistsAsync(string email, string passwordHash)
    {
        IQueryable<User> users = _applicationDbContext.Set<User>();
        return await users
            .AnyAsync(x => x.Password == passwordHash &&
                           x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
    }
}