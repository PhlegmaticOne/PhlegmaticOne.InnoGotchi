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
    public async Task<bool> ExistsAsync(string email)
    {
        var isExistsByEmail = IsExistsByEmail(email);
        return await isExistsByEmail.FirstOrDefaultAsync() is not null;
    }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        var isExistsByEmail = IsExistsByEmail(email);
        return await isExistsByEmail.FirstOrDefaultAsync();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var users = _applicationDbContext.Set<User>();
        var entity = await users.AddAsync(user);
        await _applicationDbContext.SaveChangesAsync();
        return entity.Entity;
    }

    private IQueryable<User> IsExistsByEmail(string email)
    {
        IQueryable<User> users = _applicationDbContext.Set<User>();
        return users.Where(x => x.Email.ToLower() == email.ToLower());
    }
}