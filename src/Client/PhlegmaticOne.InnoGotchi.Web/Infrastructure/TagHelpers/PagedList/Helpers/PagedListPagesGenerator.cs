using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Classes;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Classes.Base;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Helpers;

public class PagedListPagesGenerator : IPagedListPagesGenerator
{
    private const string LabelPrevious = "«";
    private const string LabelNext = "»";
    private const string LabelLast = "»»";
    private const string LabelFirst = "««";

    public IList<PagerPageBase> GeneratePages(int pageIndex, int pageSize, int totalCount, int pagesInGroup)
    {
        var pagerContext = PagerContext.FromData(pageIndex, pageSize, totalCount, pagesInGroup);
        return new List<PagerPageBase>()
            .With(PreviousPages(pagerContext))
            .With(NumberedPages(pagerContext))
            .With(NextPages(pagerContext));
    }

    private static IEnumerable<PagerPageBase> PreviousPages(PagerContext pager)
    {
        yield return pager.PageIndex == 0 ? new PagerDisabledPage(LabelFirst, 1) : new PagerPage(LabelFirst, 1);

        yield return pager.PreviousPage > 1
            ? new PagerPage(LabelPrevious, pager.MinPage - 1)
            : new PagerDisabledPage(LabelPrevious, pager.MinPage - 1);
    }

    private static IEnumerable<PagerPageBase> NumberedPages(PagerContext pager)
    {
        for (var i = pager.MinPage; i <= pager.MaxPage; i++)
        {
            if (i == pager.PageIndex + 1)
            {
                yield return new PagerActivePage(i.ToString(), i);
                continue;
            }

            yield return new PagerPage(i.ToString(), i);
        }
    }

    private static IEnumerable<PagerPageBase> NextPages(PagerContext pager)
    {
        yield return pager.NextPage >= pager.MaxPage
            ? new PagerPage(LabelNext, pager.MaxPage + 1)
            : new PagerDisabledPage(LabelNext, pager.MaxPage);

        yield return pager.PageIndex + 1 == pager.TotalPages
            ? new PagerDisabledPage(LabelLast, pager.TotalPages)
            : new PagerPage(LabelLast, pager.TotalPages);
    }
}