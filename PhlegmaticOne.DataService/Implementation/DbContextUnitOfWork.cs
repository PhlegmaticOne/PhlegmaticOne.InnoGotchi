using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        _repositories = new();
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

    public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();

    public async Task<OperationResult<T>> ResultFromExecutionInTransaction<T>(Func<Task<T>> operation)
    {
        await using var transaction = await BeginTransactionAsync();

        try
        {
            var result = await operation();
            await transaction.CommitAsync();
            return OperationResult.FromSuccess(result);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return OperationResult.FromFail<T>(e.Message);
        }
    }

    public async Task<OperationResult> ResultFromExecutionInTransaction(Func<Task> operation)
    {
        await using var transaction = await BeginTransactionAsync();

        try
        {
            await operation();
            await transaction.CommitAsync();
            return OperationResult.Success;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return OperationResult.FromFail(e.Message);
        }
    }

    private Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_dbContext.Database.CurrentTransaction is not null)
        {
            return Task.FromResult(_dbContext.Database.CurrentTransaction);
        }
        return _dbContext.Database.BeginTransactionAsync();
    }
}