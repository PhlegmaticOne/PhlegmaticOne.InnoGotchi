using Microsoft.EntityFrameworkCore.Storage;
using PhlegmaticOne.UnitOfWork.Models;

namespace PhlegmaticOne.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    IRepository<TEntity> GetDataRepository<TEntity>() where TEntity : EntityBase;
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<int> SaveChangesAsync();
}