using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Data.Core.Base;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Data.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.DataServices;

public class EfDataService<T> : IDataService<T> where T : ModelBase
{
    private readonly ApplicationDbContext _applicationDbContext;

    public EfDataService(ApplicationDbContext applicationDbContext) => 
        _applicationDbContext = applicationDbContext;

    public async Task<T> CreateAsync(T data)
    {
        var result = await Set().AddAsync(data);
        await _applicationDbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<T?> UpdateAsync(Guid id, Action<T> actionOverExistingEntity)
    {
        var existing = await GetByIdOrDefaultAsync(id);

        if (existing == null)
        {
            return null;
        }

        actionOverExistingEntity(existing);
        Set().Update(existing);
        await _applicationDbContext.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await GetByIdOrDefaultAsync(id);

        if (entity is null)
        {
            return false;
        }

        Set().Remove(entity);
        return true;
    }

    public Task<T?> GetByIdOrDefaultAsync(Guid id) => Set().FirstOrDefaultAsync(x => x.Id == id);

    public Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate) => Set().FirstOrDefaultAsync(predicate);

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate) => await Set().Where(predicate).ToListAsync();

    public async Task<IEnumerable<T>> GetAllAsync() => await Set().ToListAsync();

    public Task<bool> IsTrueAsync(Expression<Func<T, bool>> predicate) => Set().AnyAsync(predicate);

    private DbSet<T> Set() => _applicationDbContext.Set<T>();
}