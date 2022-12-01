using PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Classes.Base;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Helpers;

public interface IPagedListPagesGenerator
{
    IList<PagerPageBase> GeneratePages(int pageIndex, int pageSize, int totalCount, int pagesInGroup);
}