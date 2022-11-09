using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Services;

public class EfFarmsDataService : IFarmsDataService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public EfFarmsDataService(ApplicationDbContext applicationDbContext) => 
        _applicationDbContext = applicationDbContext;

    public Task<bool> ExistsForUserAsync(string email) => Farms().AnyAsync(WithUserEmail(email));

    public async Task<Farm> CreateAsync(Farm farm)
    {
        var entity = await Farms().AddAsync(farm);
        await _applicationDbContext.SaveChangesAsync();
        return entity.Entity;
    }

    public Task<Farm?> GetByEmailAsync(string email) => Farms()
            .Include(x => x.Collaborations)
            .Include(x => x.InnoGotchies)
            .FirstOrDefaultAsync(WithUserEmail(email));

    private DbSet<Farm> Farms() => _applicationDbContext.Set<Farm>();

    private static Expression<Func<Farm, bool>> WithUserEmail(string email) => f => f.Owner.User.Email == email;
}