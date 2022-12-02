using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase;
    Task<int> SaveChangesAsync();
    Task<OperationResult<T>> ResultFromExecutionInTransaction<T>(Func<Task<T>> operation);
    Task<OperationResult> ResultFromExecutionInTransaction(Func<Task> operation);
}