using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Constants;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Helpers;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList;

[HtmlTargetElement(PagedListTagHelperConstants.TagName,
    Attributes = PagedListTagHelperConstants.PageIndexAttributeName)]
[HtmlTargetElement(PagedListTagHelperConstants.TagName, Attributes = PagedListTagHelperConstants.PageSizeAttributeName)]
[HtmlTargetElement(PagedListTagHelperConstants.TagName,
    Attributes = PagedListTagHelperConstants.TotalCountAttributeName)]
[HtmlTargetElement(PagedListTagHelperConstants.TagName, Attributes = PagedListTagHelperConstants.ActionAttributeName)]
[HtmlTargetElement(PagedListTagHelperConstants.TagName, Attributes = PagedListTagHelperConstants.HostAttributeName)]
[HtmlTargetElement(PagedListTagHelperConstants.TagName, Attributes = PagedListTagHelperConstants.FragmentAttributeName)]
[HtmlTargetElement(PagedListTagHelperConstants.TagName, Attributes = PagedListTagHelperConstants.RouteAttributeName)]
[HtmlTargetElement(PagedListTagHelperConstants.TagName,
    Attributes = PagedListTagHelperConstants.RouteDataAttributeName)]
[HtmlTargetElement(PagedListTagHelperConstants.TagName, Attributes = PagedListTagHelperConstants.ProtocolAttributeName)]
[HtmlTargetElement(PagedListTagHelperConstants.TagName,
    Attributes = PagedListTagHelperConstants.ControllerAttributeName)]
public class PagedListTagHelper : TagHelper
{
    private const string DisableCss = "disabled";
    private const string PageLinkCss = "page-link";
    private const string RootTagCss = "pagination";
    private const string ActiveTagCss = "active";
    private const string PageItemCss = "page-item";
    private const byte VisibleGroupCount = 10;
    private readonly IHtmlGenerator _htmlGenerator;


    private readonly IPagedListPagesGenerator _pagedListPagesGenerator;
    private readonly Dictionary<string, string> _routeValues = new();

    public PagedListTagHelper(IPagedListPagesGenerator pagedListPagesGenerator, IHtmlGenerator htmlGenerator)
    {
        _pagedListPagesGenerator = pagedListPagesGenerator;
        _htmlGenerator = htmlGenerator;
    }

    [HtmlAttributeName(PagedListTagHelperConstants.PageIndexAttributeName)]
    public int PagedListIndex { get; set; }

    [HtmlAttributeName(PagedListTagHelperConstants.PageSizeAttributeName)]
    public int PagedListSize { get; set; }

    [HtmlAttributeName(PagedListTagHelperConstants.TotalCountAttributeName)]
    public int PagedListTotalCount { get; set; }

    [HtmlAttributeName(PagedListTagHelperConstants.ActionAttributeName)]
    public string? ActionName { get; set; }

    [HtmlAttributeName(PagedListTagHelperConstants.RouteAttributeName)]
    public string RouteParameter { get; set; } = null!;

    [HtmlAttributeName(PagedListTagHelperConstants.RouteDataAttributeName)]
    public object? RouteParameters { get; set; }

    [HtmlAttributeName(PagedListTagHelperConstants.FragmentAttributeName)]
    public string? Fragment { get; set; }

    [HtmlAttributeName(PagedListTagHelperConstants.ProtocolAttributeName)]
    public string? Protocol { get; set; }

    [HtmlAttributeName(PagedListTagHelperConstants.HostAttributeName)]
    public string? Host { get; set; }

    [HtmlAttributeName(PagedListTagHelperConstants.ControllerAttributeName)]
    public string Controller { get; set; } = null!;

    [ViewContext] public ViewContext? ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (PagedListTotalCount < 1) return;

        var ul = new TagBuilder("ul");
        ul.AddCssClass(RootTagCss);

        var pages = _pagedListPagesGenerator
            .GeneratePages(PagedListIndex, PagedListSize, PagedListTotalCount, VisibleGroupCount);

        foreach (var page in pages)
        {
            var li = new TagBuilder("li");
            li.AddCssClass(PageItemCss);

            if (page.IsActive) li.AddCssClass(ActiveTagCss);

            if (page.IsDisabled) li.AddCssClass(DisableCss);

            li.InnerHtml.AppendHtml(GenerateLink(page.Title, page.Value.ToString()));
            ul.InnerHtml.AppendHtml(li);
        }

        output.Content.AppendHtml(ul);
        base.Process(context, output);
    }

    private TagBuilder GenerateLink(string linkText, string routeValue)
    {
        var routeValues = new RouteValueDictionary(_routeValues)
        {
            { RouteParameter, routeValue }
        };

        if (RouteParameters is not null)
        {
            var values = RouteParameters.GetType().GetProperties();
            if (values.Any())
                foreach (var propertyInfo in values)
                    routeValues.Add(propertyInfo.Name, propertyInfo.GetValue(RouteParameters));
        }

        return _htmlGenerator.GenerateActionLink(
            ViewContext,
            actionName: ActionName,
            controllerName: Controller,
            routeValues: routeValues,
            hostname: Host,
            linkText: linkText,
            fragment: Fragment,
            htmlAttributes: new { @class = PageLinkCss },
            protocol: Protocol);
    }
}