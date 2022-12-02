namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Classes;

public class PagerContext
{
    public int GroupIndex { get; set; }

    public int MinPage { get; set; }

    public int MaxPage { get; set; }

    public int NextPage { get; set; }

    public int PreviousPage { get; set; }

    public int TotalPages { get; set; }

    public int PageIndex { get; set; }

    public static PagerContext FromData(int pageIndex, int pageSize, int totalCount, int pagesInGroup)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var groupIndex = (int)Math.Floor(Convert.ToDecimal(pageIndex) / Convert.ToDecimal(pagesInGroup));
        var minPage = groupIndex * pagesInGroup + 1;
        var maxPage = minPage + pagesInGroup - 1;
        var prevPage = minPage - 1;
        var nextPage = maxPage + 1;

        if (minPage <= 1) prevPage = 1;

        if (maxPage > totalPages) maxPage = totalPages;

        if (nextPage > totalPages) nextPage = 0;

        return new PagerContext
        {
            PageIndex = pageIndex,
            TotalPages = totalPages,
            GroupIndex = groupIndex,
            MinPage = minPage,
            MaxPage = maxPage,
            NextPage = nextPage,
            PreviousPage = prevPage
        };
    }
}