using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Services;

public class EfInnoGotchiComponentsDataService : IInnoGotchiComponentsDataService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public EfInnoGotchiComponentsDataService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public async Task<IEnumerable<InnoGotchiComponent>> GetAllAsync()
    {
        var components = await _applicationDbContext.Set<InnoGotchiComponent>().ToListAsync();
        return components;
    }
}