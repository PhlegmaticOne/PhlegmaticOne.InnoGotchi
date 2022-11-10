using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using PhlegmaticOne.InnoGotchi.Data.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Data.Core.Base;

public interface IDataRepository<T> where T : ModelBase
{
    Task<T> CreateAsync(T data);
    Task<T?> UpdateAsync(Guid id, Action<T> actionOverExistingEntity);
    Task<bool> DeleteAsync(Guid id);
    Task<T?> GetByIdOrDefaultAsync(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
}