namespace PhlegmaticOne.InnoGotchi.Domain.Services;

public interface ISortingService<T>
{
    Func<IQueryable<T>, IOrderedQueryable<T>> GetSortingFunc(int sortType, bool isAscending);
}