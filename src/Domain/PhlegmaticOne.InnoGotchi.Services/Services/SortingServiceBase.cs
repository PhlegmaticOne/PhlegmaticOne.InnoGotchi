using PhlegmaticOne.InnoGotchi.Domain.Services;
using System.Linq.Expressions;

namespace PhlegmaticOne.InnoGotchi.Services.Services;

public class SortingServiceBase<T> : ISortingService<T>
{
    private readonly Dictionary<int, Expression<Func<T, object>>> _sortByProperties;

    public SortingServiceBase(Dictionary<int, Expression<Func<T, object>>> sortByProperties) =>
        _sortByProperties = sortByProperties;

    public Func<IQueryable<T>, IOrderedQueryable<T>> GetSortingFunc(int sortType, bool isAscending)
    {
        if (_sortByProperties.TryGetValue(sortType, out var sortByProperty) == false)
        {
            sortByProperty = _sortByProperties[0];
        }

        return isAscending ? query => query.OrderBy(sortByProperty) : query => query.OrderByDescending(sortByProperty);
    }
}