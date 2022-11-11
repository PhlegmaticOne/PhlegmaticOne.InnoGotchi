using PhlegmaticOne.PagedList.Base;

namespace PhlegmaticOne.PagedList;

public class PagedList<T> : IPagedList<T>
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages { get; init; }

    public int IndexFrom { get; init; }

    public IList<T> Items { get; init; } = null!;

    public bool HasPreviousPage => PageIndex - IndexFrom > 0;

    public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;
}