using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;
using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.UnitOfWork.Implementation;

public class DbContextUnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private readonly Dictionary<Type, object> _repositories;

    public DbContextUnitOfWork(DbContext dbContext)
    {
        _repositories = new Dictionary<Type, object>();
        _dbContext = dbContext;
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase
    {
        var type = typeof(TEntity);

        if (_repositories.ContainsKey(type) == false)
        {
            var set = _dbContext.Set<TEntity>();
            _repositories[type] = new DbSetRepository<TEntity>(set);
        }

        return (IRepository<TEntity>)_repositories[type];
    }

    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }

    public async Task<OperationResult<T>> ResultFromExecutionInTransaction<T>(Func<Task<T>> operation)
    {
        try
        {
            var result = await operation();
            await SaveChangesAsync();
            return OperationResult.FromSuccess(result);
        }
        catch (Exception e)
        {
            return OperationResult.FromFail<T>(e.Message);
        }
    }

    public async Task<OperationResult> ResultFromExecutionInTransaction(Func<Task> operation)
    {
        try
        {
            await operation();
            await SaveChangesAsync();
            return OperationResult.Success;
        }
        catch (Exception e)
        {
            return OperationResult.FromFail(e.Message);
        }
    }
}