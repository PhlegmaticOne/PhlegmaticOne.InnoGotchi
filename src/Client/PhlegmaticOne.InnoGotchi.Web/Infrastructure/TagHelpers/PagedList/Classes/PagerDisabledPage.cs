using PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Classes.Base;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Classes;

public class PagerDisabledPage : PagerPageBase
{
    public PagerDisabledPage(string title, int value) : base(title, value, false, true)
    {
    }
}