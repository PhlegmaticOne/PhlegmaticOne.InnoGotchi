namespace PhlegmaticOne.InnoGotchi.Shared.PagedList;

public class PagedListData
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int SortType { get; set; }
    public bool IsAscending { get; set; }
}