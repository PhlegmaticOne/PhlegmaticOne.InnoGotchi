using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PhlegmaticOne.UnitOfWork.Interfaces;
using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.UnitOfWork.Implementation;

public class DbContextUnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private readonly Dictionary<Type, object> _repositories;
    public DbContextUnitOfWork(DbContext dbContext)
    {
        _repositories = new();
        _dbContext = dbContext;
    }

    public IRepository<TEntity> GetDataRepository<TEntity>() where TEntity : EntityBase
    {
        var type = typeof(TEntity);

        if (_repositories.ContainsKey(type) == false)
        {
            var set = _dbContext.Set<TEntity>();
            _repositories[type] = new DbSetRepository<TEntity>(set);
        }

        return (IRepository<TEntity>)_repositories[type];
    }

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_dbContext.Database.CurrentTransaction is not null)
        {
            return Task.FromResult(_dbContext.Database.CurrentTransaction);
        }
        return _dbContext.Database.BeginTransactionAsync();
    }

    public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();
}