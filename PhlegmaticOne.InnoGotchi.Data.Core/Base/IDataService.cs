using System.Linq.Expressions;
using PhlegmaticOne.InnoGotchi.Data.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Data.Core.Base;

public interface IDataService<T> where T : ModelBase
{
    Task<T> CreateAsync(T data);
    Task<T?> UpdateAsync(Guid id, Action<T> actionOverExistingEntity);
    Task<bool> DeleteAsync(Guid id);
    Task<T?> GetByIdOrDefaultAsync(Guid id);
    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> IsTrueAsync(Expression<Func<T, bool>> predicate);
}