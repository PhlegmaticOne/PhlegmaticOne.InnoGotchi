using PhlegmaticOne.DataService.Models;

namespace PhlegmaticOne.DataService.Interfaces;

public interface IDataService
{
    IDataRepository<TEntity> GetDataRepository<TEntity>() where TEntity : EntityBase;
    Task<int> SaveChangesAsync();
}