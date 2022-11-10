using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.DataService.Models;

namespace PhlegmaticOne.DataService.Implementation;

public class DbContextDataService : IDataService
{
    private readonly DbContext _dbContext;
    private readonly Dictionary<Type, object> _repositories;
    public DbContextDataService(DbContext dbContext)
    {
        _repositories = new();
        _dbContext = dbContext;
    }

    public IDataRepository<TEntity> GetDataRepository<TEntity>() where TEntity : EntityBase
    {
        var type = typeof(TEntity);

        if (_repositories.ContainsKey(type) == false)
        {
            var set = _dbContext.Set<TEntity>();
            _repositories[type] = new DbSetDataRepository<TEntity>(set);
        }

        return (IDataRepository<TEntity>)_repositories[type];
    }

    public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();
}