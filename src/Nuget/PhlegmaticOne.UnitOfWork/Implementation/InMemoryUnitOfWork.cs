using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;
using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.UnitOfWork.Implementation;

public class InMemoryUnitOfWork : IUnitOfWork
{
    private readonly Dictionary<Type, IRepository> _repositories;
    public InMemoryUnitOfWork() => _repositories = new Dictionary<Type, IRepository>();

    public InMemoryUnitOfWork(IEnumerable<IRepository> repositories) =>
        _repositories = repositories.ToDictionary(
            key => key.GetType().GetGenericArguments().First(),
            value => value);

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase
    {
        var type = typeof(TEntity);

        if (_repositories.ContainsKey(type) == false)
        {
            _repositories[type] = new InMemoryRepository<TEntity>();
        }

        return (IRepository<TEntity>)_repositories[type];
    }

    public Task<int> SaveChangesAsync() => Task.FromResult(0);

    public async Task<OperationResult<T>> ResultFromExecutionInTransaction<T>(Func<Task<T>> operation)
    {
        try
        {
            var result = await operation();
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
            return OperationResult.Success;
        }
        catch (Exception e)
        {
            return OperationResult.FromFail(e.Message);
        }
    }
}